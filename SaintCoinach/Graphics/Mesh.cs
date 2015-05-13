using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class Mesh {
        public const byte BytesPerIndex = 2;   // TODO: 99.999% sure this is constant, but you never know.

        #region Fields

        #endregion

        #region Properties
        public Model Model { get; private set; }
        public MeshHeader Header { get { return Model.Definition.MeshHeaders[Index]; } }
        public VertexFormat VertexFormat { get { return Model.Definition.VertexFormats[Index]; } }
        public int Index { get; private set; }
        public Vertex[] Vertices { get; private set; }
        public ushort[] Indices { get; private set; }
        public MeshPart[] Parts { get; private set; }
        public MaterialDefinition Material { get { return Model.Definition.Materials[Header.MaterialIndex]; } }
        #endregion

        #region Constructor
        public Mesh(Model model, int index, byte[] vertexBuffer, byte[] indexBuffer) {
            this.Model = model;
            this.Index = index;

            this.Parts = new MeshPart[Header.PartCount];
            for (var i = 0; i < Header.PartCount; ++i)
                this.Parts[i] = new MeshPart(this, Model.Definition.MeshPartHeaders[Header.PartOffset + i], indexBuffer);

            ReadVertices(vertexBuffer);
            ReadIndices(indexBuffer);
        }
        #endregion

        #region Build
        private void ReadIndices(byte[] buffer) {
            var position = Header.IndexBufferOffset * BytesPerIndex;
            this.Indices = new ushort[Header.IndexCount];
            for (var i = 0; i < Header.IndexCount; ++i) {
                this.Indices[i] = BitConverter.ToUInt16(buffer, position);
                position += BytesPerIndex;
            }
        }
        private void ReadVertices(byte[] buffer) {
            var header = Header;
            var format = VertexFormat;

            var offsets = new int[header.VertexBufferCount];
            for (var oi = 0; oi < offsets.Length; ++oi)
                offsets[oi] = header.VertexOffsets[oi];

            Vertices = new Vertex[Header.VertexCount];
            for (var i = 0; i < header.VertexCount; ++i) {
                Vertices[i] = VertexReader.Read(buffer, format, offsets);
                
                for (var oi = 0; oi < offsets.Length; ++oi)
                    offsets[oi] += header.BytesPerVertexPerBuffer[oi];
            }
        }
        #endregion
    }
}
