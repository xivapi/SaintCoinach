using System;

namespace SaintCoinach.Graphics.Assets {
    public class MeshHeader {
        #region Static

        public const int Size = 0x24;

        #endregion

        #region Properties

        public int VertexCount { get; private set; }
        public int IndexCount { get; private set; }
        public int MaterialFileIndex { get; private set; }
        public int IndexOffset { get; private set; }
        public int VertexOffset { get; private set; }
        public int BytesPerVertexPosition { get; private set; }
        public int BytesPerVertexData { get; private set; }
        public int BytesPerIndex { get; private set; }
        public string Name { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        internal MeshHeader(string name, byte[] buffer, int offset) {
            const int VertexCountOffset = 0x00;
            const int IndexCountOffset = 0x04;
            const int MaterialOffset = 0x08;
            const int IndexPositionOffset = 0x10;
            const int VertexPositionOffset = 0x14;
            const int BytesPerVertexPositionOffset = 0x20;
            const int BytesPerVDataOffset = 0x21;
            const int BytesPerIndexOffset = 0x23;

            Name = name;

            VertexCount = BitConverter.ToInt32(buffer, offset + VertexCountOffset);
            IndexCount = BitConverter.ToInt32(buffer, offset + IndexCountOffset);
            MaterialFileIndex = BitConverter.ToUInt16(buffer, offset + MaterialOffset);

            IndexOffset = BitConverter.ToInt32(buffer, offset + IndexPositionOffset);
            VertexOffset = BitConverter.ToInt32(buffer, offset + VertexPositionOffset);

            BytesPerVertexPosition = buffer[offset + BytesPerVertexPositionOffset];
            BytesPerVertexData = buffer[offset + BytesPerVDataOffset];
            //_BytesPerIndex = data[offset + BytesPerIndexOffset];  // Newp, something else
            // TODO: Figure out what the rest is
            BytesPerIndex = 2;
        }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
