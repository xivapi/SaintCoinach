using System;

namespace SaintCoinach.Graphics.Assets {
    public class SubModelHeader {
        #region Static

        public const int Size = 0x3C;

        #endregion

        #region Properties

        public int MeshOffset { get; private set; }
        public int MeshCount { get; private set; }
        public int VertexDataLength { get; private set; }
        public int IndexDataLength { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public SubModelHeader(byte[] buffer, int offset) {
            const int MeshPositionOffset = 0x00;
            const int MeshCountOffset = 0x02;
            const int VertexDataLengthOffset = 0x2C;
            const int IndexDataLengthOffset = 0x30;

            MeshOffset = BitConverter.ToUInt16(buffer, offset + MeshPositionOffset);
            MeshCount = BitConverter.ToUInt16(buffer, offset + MeshCountOffset);
            VertexDataLength = BitConverter.ToInt32(buffer, offset + VertexDataLengthOffset);
            IndexDataLength = BitConverter.ToInt32(buffer, offset + IndexDataLengthOffset);
        }

        #endregion

        #endregion
    }
}
