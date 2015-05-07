using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class MeshPart {
        #region Properties
        public Mesh Mesh { get; private set; }
        public MeshPartHeader Header { get; private set; }
        public ushort[] Indices { get; private set; }
        #endregion

        #region Constructor
        internal MeshPart(Mesh mesh, MeshPartHeader header, byte[] indexBuffer) {
            const byte BytesPerIndex = 2;   // TODO: 99.999% sure this is constant, but you never know.

            this.Mesh = mesh;
            this.Header = header;

            var position = header.IndexOffset * BytesPerIndex;
            this.Indices = new ushort[header.IndexCount];
            for (var i = 0; i < header.IndexCount; ++i) {
                this.Indices[i] = BitConverter.ToUInt16(indexBuffer, position);
                position += BytesPerIndex;
            }
        }
        #endregion
    }
}
