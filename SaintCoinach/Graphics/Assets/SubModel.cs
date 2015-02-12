using System.Collections.Generic;

namespace SaintCoinach.Graphics.Assets {
    public class SubModel {
        #region Fields

        private readonly Mesh[] _Meshes;

        #endregion

        #region Properties

        public Model Model { get; private set; }
        public SubModelHeader Header { get; private set; }
        public IEnumerable<Mesh> Meshes { get { return _Meshes; } }

        #endregion

        #region Constructors

        #region Constructor

        public SubModel(Model model, SubModelHeader header, byte[] vertexData, byte[] indexData) {
            Model = model;
            Header = header;

            _Meshes = new Mesh[Header.MeshCount];
            for (var i = 0; i < Header.MeshCount; ++i) {
                var mesh = Model.GetMesh(Header.MeshOffset + i);

                mesh.ReadVertices(vertexData);
                mesh.ReadIndices(indexData);

                _Meshes[i] = mesh;
            }
        }

        #endregion

        #endregion
    }
}
