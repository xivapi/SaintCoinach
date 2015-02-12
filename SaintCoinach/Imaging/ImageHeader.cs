using System;
using System.IO;

namespace SaintCoinach.Imaging {
    /// <summary>
    ///     Header of an image file inside SqPack.
    /// </summary>
    public class ImageHeader {
        #region Fields

        #region Constructor

        internal byte[] _Buffer;

        #endregion

        #endregion

        #region Properties

        public int Width { get; private set; }
        public int Height { get; private set; }
        public ImageFormat Format { get; private set; }
        public long EndOfHeader { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public ImageHeader(Stream stream) {
            const int Length = 0x50;

            const int FormatOffset = 0x04;
            const int WidthOffset = 0x08;
            const int HeightOffset = 0x0A;

            _Buffer = new byte[Length];
            if (stream.Read(_Buffer, 0, Length) != Length)
                throw new EndOfStreamException();

            Width = BitConverter.ToInt16(_Buffer, WidthOffset);
            Height = BitConverter.ToInt16(_Buffer, HeightOffset);
            Format = (ImageFormat)BitConverter.ToInt16(_Buffer, FormatOffset);
            EndOfHeader = stream.Position;
        }

        #endregion

        #endregion

        public byte[] GetBuffer() {
            var b = new byte[_Buffer.Length];
            Array.Copy(_Buffer, b, b.Length);
            return b;
        }
    }
}
