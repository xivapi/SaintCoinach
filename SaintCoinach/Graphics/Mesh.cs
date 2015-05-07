using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class Mesh {
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
        }
        #endregion

        #region Build
        private void ReadVertices(byte[] buffer) {
            var header = Header;
            var format = VertexFormat;

            var p1Offset = header.VertexData1Offset;
            var p2Offset = header.VertexData2Offset;

            Vertices = new Vertex[Header.VertexCount];
            for (var i = 0; i < header.VertexCount; ++i) {
                Vertices[i] = VertexReader.Read(buffer, format, p1Offset, p2Offset);

                p1Offset += header.BytesPerVertexData1;
                p2Offset += header.BytesPerVertexData2;
            }
        }
        #endregion
    }
}
