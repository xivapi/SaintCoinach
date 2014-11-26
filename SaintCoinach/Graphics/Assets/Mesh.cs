using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Assets {
    public class Mesh {
        #region Fields
        private Model _Model;
        private MeshHeader _Header;
        private int _Type;
        private Material _Material;
        private ushort[] _Indices;
        private VertexBase[] _Vertices;
        private Type _VertexType;
        #endregion

        #region Properties
        public Model Model { get { return _Model; } }
        public MeshHeader Header { get { return _Header; } }
        public int Type { get { return _Type; } }
        public Type VertexType { get { return _VertexType; } }
        public Material Material { get { return _Material; } }

        public ushort[] GetIndices() {
            var b = new ushort[_Indices.Length];
            Array.Copy(_Indices, b, b.Length);
            return b;
        }
        public VertexBase[] GetVertices() {
            var b = new VertexBase[_Vertices.Length];
            Array.Copy(_Vertices, b, b.Length);
            return b;
        }
        #endregion

        #region Constructor
        public Mesh(Model model, MeshHeader header, byte[] data, int index) {
            const int DataBlockLength = 0x88;
            const int TypeOffset = 0x00;

            _Model = model;
            _Header = header;
            _Type = BitConverter.ToInt32(data, index * DataBlockLength + TypeOffset);
            _Material = Model.GetMaterial(Header.MaterialFileIndex);
        }
        #endregion

        #region Vertices
        public void ReadVertices(byte[] vertexData) {
            _Vertices = VertexReader.Read(this, vertexData, out _VertexType);
        }
        #endregion

        #region Indices
        public void ReadIndices(byte[] indexData) {
            var off = Header.IndexOffset * Header.BytesPerIndex;

            _Indices = new ushort[Header.IndexCount];
            for (int i = 0; i < Header.IndexCount; ++i) {
                _Indices[i] = BitConverter.ToUInt16(indexData, off);
                off += Header.BytesPerIndex;
            }
        }
        #endregion
    }
}
