using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class Model {
        #region Fields
        private ModelFile _File;
        private ModelHeader _Header;
        private SubModel[] _SubModels;
        private Mesh[] _Meshes;
        private Material[] _Materials;
        #endregion

        #region Properties
        public ModelFile File { get { return _File; } }
        public ModelHeader Header { get { return _Header; } }
        public int MaterialCount { get { return _Materials.Length; } }

        public Material GetMaterial(int index) { return _Materials[index]; }
        public Mesh GetMesh(int index) { return _Meshes[index]; }
        public SubModel GetSubModel(int index) { return _SubModels[index]; }
        public SubModel GetSubModel(ModelQuality quality) { return GetSubModel((int)quality); }
        #endregion

        #region Constructor
        public Model(ModelFile file) {
            const int HeaderPart = 1;
            const int VertexPartOffset = 2;
            const int IndexPartOffset = 8;

            _File = file;

            _Header = new ModelHeader(File.GetPart(HeaderPart));

            BuildMaterials();

            BuildMeshBase();

            _SubModels = new SubModel[Header.SubModelCount];
            for(int i = 0; i < Header.SubModelCount; ++i) {
                var vertexPart = File.GetPart(VertexPartOffset + i);
                var indexPart = File.GetPart(IndexPartOffset + i);
                _SubModels[i] = new SubModel(this, Header.GetSubModelHeader(i), vertexPart, indexPart);
            }
        }

        private void BuildMaterials() {
            _Materials = new Material[Header.MaterialCount];
            for (int i = 0; i < Header.MaterialCount; ++i)
                _Materials[i] = new Material(this, Header.GetMaterialName(i));
        }
        private void BuildMeshBase() {
            const int MeshTypesPart = 0;

            var part = File.GetPart(MeshTypesPart);

            _Meshes = new Mesh[Header.MeshCount];
            for (int i = 0; i < Header.MeshCount; ++i)
                _Meshes[i] = new Mesh(this, Header.GetMeshHeader(i), part, i);
        }
        #endregion
    }
}
