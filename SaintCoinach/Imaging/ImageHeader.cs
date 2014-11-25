using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Imaging {
    /// <summary>
    /// Header of an image file inside SqPack.
    /// </summary>
    public class ImageHeader {
        #region Constructor
        internal byte[] _Buffer;

        private int _Width;
        private int _Height;
        private ImageFormat _Format;
        private long _EndOfHeader;
        #endregion

        #region Properties
        public int Width { get { return _Width; } }
        public int Height { get { return _Height; } }
        public ImageFormat Format { get { return _Format; } }
        public long EndOfHeader { get { return _EndOfHeader; } }
        public byte[] GetBuffer() {
            var b = new byte[_Buffer.Length];
            Array.Copy(_Buffer, b, b.Length);
            return b;
        }
        #endregion

        #region Constructor
        public ImageHeader(Stream stream) {
            const int Length = 0x50;

            const int FormatOffset = 0x04;
            const int WidthOffset = 0x08;
            const int HeightOffset = 0x0A;

            _Buffer = new byte[Length];
            if (stream.Read(_Buffer, 0, Length) != Length)
                throw new EndOfStreamException();

            _Width = BitConverter.ToInt16(_Buffer, WidthOffset);
            _Height = BitConverter.ToInt16(_Buffer, HeightOffset);
            _Format = (ImageFormat)BitConverter.ToInt16(_Buffer, FormatOffset);
            _EndOfHeader = stream.Position;
        }
        #endregion
    }
}
