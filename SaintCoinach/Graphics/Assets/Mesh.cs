using System;
using System.Linq;

namespace SaintCoinach.Graphics.Assets {
    public class Mesh {
        #region Fields

        internal ushort[] _Indices;
        private Type _VertexType;
        internal VertexBase[] _Vertices;
        internal MeshPartHeader[] _Parts;

        #endregion

        #region Properties

        public Model Model { get; private set; }
        public MeshHeader Header { get; private set; }
        public int Type { get; private set; }
        public Type VertexType { get { return _VertexType; } }
        public Material Material { get; private set; }
        public int PartCount { get { return _Parts.Length; } }
        public MeshPartHeader GetPart(int index) {
            return _Parts[index];
        }

        #endregion

        #region Constructors

        public Mesh(Model model, MeshHeader header, byte[] data, int index) {
            const int DataBlockLength = 0x88;
            const int TypeOffset = 0x00;

            Model = model;
            Header = header;
            Type = BitConverter.ToInt32(data, index * DataBlockLength + TypeOffset);
            Material = Model.GetMaterial(Header.MaterialFileIndex);
            _Parts = new MeshPartHeader[Header.PartCount];
            for (var i = 0; i < Header.PartCount; ++i)
                _Parts[i] = Model.Header.GetMeshPartHeader(i + Header.PartOffset);
        }

        #endregion

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

        public T[] GetVertices<T>() where T : VertexBase {
            return _Vertices.Cast<T>().ToArray();
        }

        #region Vertices

        public void ReadVertices(byte[] vertexData) {
            _Vertices = VertexReader.Read(this, vertexData, out _VertexType);
        }

        #endregion

        #region Indices

        public void ReadIndices(byte[] indexData) {
            var off = Header.IndexOffset * Header.BytesPerIndex;

            _Indices = new ushort[Header.IndexCount];
            for (var i = 0; i < Header.IndexCount; ++i) {
                _Indices[i] = BitConverter.ToUInt16(indexData, off);
                off += Header.BytesPerIndex;
            }
        }

        #endregion
    }
}
