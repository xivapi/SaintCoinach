using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class SubModelHeader {
        public const int Size = 0x3C;

        #region Fields
        private int _MeshOffset;
        private int _MeshCount;
        private int _VertexDataLength;
        private int _IndexDataLength;
        #endregion

        #region Properties
        public int MeshOffset { get { return _MeshOffset; } }
        public int MeshCount { get { return _MeshCount; } }
        public int VertexDataLength { get { return _VertexDataLength; } }
        public int IndexDataLength { get { return _IndexDataLength; } }
        #endregion
        
        #region Constructor
        public SubModelHeader(byte[] buffer, int offset) {
            const int MeshPositionOffset = 0x00;
            const int MeshCountOffset = 0x02;
            const int VertexDataLengthOffset = 0x2C;
            const int IndexDataLengthOffset = 0x30;

            _MeshOffset = BitConverter.ToUInt16(buffer, offset + MeshPositionOffset);
            _MeshCount = BitConverter.ToUInt16(buffer, offset + MeshCountOffset);
            _VertexDataLength = BitConverter.ToInt32(buffer, offset + VertexDataLengthOffset);
            _IndexDataLength = BitConverter.ToInt32(buffer, offset + IndexDataLengthOffset);

        }
        #endregion
    }
}
