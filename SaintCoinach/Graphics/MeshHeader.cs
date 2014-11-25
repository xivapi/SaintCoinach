using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class MeshHeader {
        public const int Size = 0x24;

        #region Fields
        private int _VertexCount;
        private int _IndexCount;
        private int _MaterialFileIndex;
        private int _IndexOffset;
        private int _VertexOffset;
        private int _BytesPerVertexPosition;
        private int _BytesPerVertexData;
        private int _BytesPerIndex;
        private string _Name;
        #endregion

        #region Properties
        public int VertexCount { get { return _VertexCount; } }
        public int IndexCount { get { return _IndexCount; } }
        public int MaterialFileIndex { get { return _MaterialFileIndex; } }
        public int IndexOffset { get { return _IndexOffset; } }
        public int VertexOffset { get { return _VertexOffset; } }
        public int BytesPerVertexPosition { get { return _BytesPerVertexPosition; } }
        public int BytesPerVertexData { get { return _BytesPerVertexData; } }
        public int BytesPerIndex { get { return _BytesPerIndex; } }
        public string Name { get { return _Name; } }
        #endregion

        #region Constructor
        internal MeshHeader(string name, byte[] buffer, int offset) {
            const int VertexCountOffset = 0x00;
            const int IndexCountOffset = 0x04;
            const int MaterialOffset = 0x08;
            const int IndexPositionOffset = 0x10;
            const int VertexPositionOffset = 0x14;
            const int BytesPerVPOffset = 0x20;
            const int BytesPerVDataOffset = 0x21;
            const int BytesPerIndexOffset = 0x23;

            _Name = name;

            _VertexCount = BitConverter.ToInt32(buffer, offset + VertexCountOffset);
            _IndexCount = BitConverter.ToInt32(buffer, offset + IndexCountOffset);
            _MaterialFileIndex = BitConverter.ToUInt16(buffer, offset + MaterialOffset);

            _IndexOffset = BitConverter.ToInt32(buffer, offset + IndexPositionOffset);
            _VertexOffset = BitConverter.ToInt32(buffer, offset + VertexPositionOffset);

            _BytesPerVertexPosition = buffer[offset + BytesPerVPOffset];
            _BytesPerVertexData = buffer[offset + BytesPerVDataOffset];
            //_BytesPerIndex = data[offset + BytesPerIndexOffset];  // Newp, something else
            // TODO: Figure out what the rest is
            _BytesPerIndex = 2;
        }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
