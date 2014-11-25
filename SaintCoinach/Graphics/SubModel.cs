using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class SubModel {
        #region Fields
        private Model _Model;
        private SubModelHeader _Header;
        private Mesh[] _Meshes;
        #endregion

        #region Properties
        public Model Model { get { return _Model; } }
        public SubModelHeader Header { get { return _Header; } }
        public IEnumerable<Mesh> Meshes { get { return _Meshes; } }
        #endregion

        #region Constructor
        public SubModel(Model model, SubModelHeader header, byte[] vertexData, byte[] indexData) {
            _Model = model;
            _Header = header;

            _Meshes = new Mesh[Header.MeshCount];
            for (int i = 0; i < Header.MeshCount; ++i) {
                var mesh = Model.GetMesh(Header.MeshOffset + i);

                mesh.ReadVertices(vertexData);
                mesh.ReadIndices(indexData);
                    
                _Meshes[i] = mesh;
            }
        }
        #endregion
    }
}
