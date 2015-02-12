namespace SaintCoinach.Graphics.Assets {
    public class Model {
        #region Fields

        private readonly SubModel[] _SubModels;
        private Material[] _Materials;
        private Mesh[] _Meshes;

        #endregion

        #region Properties

        public ModelFile File { get; private set; }
        public ModelHeader Header { get; private set; }
        public int MaterialCount { get { return _Materials.Length; } }

        #endregion

        public Material GetMaterial(int index) {
            return _Materials[index];
        }

        public Mesh GetMesh(int index) {
            return _Meshes[index];
        }

        public SubModel GetSubModel(int index) {
            return _SubModels[index];
        }

        public SubModel GetSubModel(ModelQuality quality) {
            return GetSubModel((int)quality);
        }

        #region Constructor

        public Model(ModelFile file) {
            const int HeaderPart = 1;
            const int VertexPartOffset = 2;
            const int IndexPartOffset = 8;

            File = file;

            Header = new ModelHeader(File.GetPart(HeaderPart));

            BuildMaterials();

            BuildMeshBase();

            _SubModels = new SubModel[Header.SubModelCount];
            for (var i = 0; i < Header.SubModelCount; ++i) {
                var vertexPart = File.GetPart(VertexPartOffset + i);
                var indexPart = File.GetPart(IndexPartOffset + i);
                _SubModels[i] = new SubModel(this, Header.GetSubModelHeader(i), vertexPart, indexPart);
            }
        }

        private void BuildMaterials() {
            _Materials = new Material[Header.MaterialCount];
            for (var i = 0; i < Header.MaterialCount; ++i)
                _Materials[i] = new Material(this, Header.GetMaterialName(i));
        }

        private void BuildMeshBase() {
            const int MeshTypesPart = 0;

            var part = File.GetPart(MeshTypesPart);

            _Meshes = new Mesh[Header.MeshCount];
            for (var i = 0; i < Header.MeshCount; ++i)
                _Meshes[i] = new Mesh(this, Header.GetMeshHeader(i), part, i);
        }

        #endregion
    }
}
