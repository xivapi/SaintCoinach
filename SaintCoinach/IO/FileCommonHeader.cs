using System;
using System.IO;

namespace SaintCoinach.IO {
    /// <summary>
    ///     Shared header for a file inside SqPack.
    /// </summary>
    public class FileCommonHeader {
        #region Fields

        internal byte[] _Buffer;

        #endregion

        #region Properties

        public IIndexFile Index { get; private set; }
        public FileType FileType { get; private set; }
        public long Length { get; private set; }
        public long EndOfHeader { get; private set; }

        #endregion

        #region Constructors

        public FileCommonHeader(IIndexFile index, Stream stream) {
            if (index == null)
                throw new ArgumentNullException("index");
            if (stream == null)
                throw new ArgumentNullException("stream");

            Index = index;

            Read(stream);
        }

        #endregion

        public byte[] GetBuffer() {
            var b = new byte[_Buffer.Length];
            Array.Copy(_Buffer, b, b.Length);
            return b;
        }

        #region Read

        private void Read(Stream stream) {
            const int FileTypeOffset = 0x04;
            const int FileLengthOffset = 0x10;
            const int FileLengthShift = 7;
            const int MinHeaderLength = 4;

            if (!stream.CanSeek)
                throw new NotSupportedException("Stream must be able to seek.");

            _Buffer = new byte[4];
            if (stream.Read(_Buffer, 0, 4) != 4)
                throw new EndOfStreamException();

            var length = BitConverter.ToInt32(_Buffer, 0);
            var remaining = stream.Length - stream.Position;
            if (length < MinHeaderLength || length > remaining + MinHeaderLength) {
                throw new InvalidDataException(
                    string.Format(
                        "Invalid SqPack file header length {0} at dat {1} offset 0x{2:X}. Remaining bytes: {3}.",
                        length,
                        Index.DatFile,
                        Index.Offset,
                        remaining + MinHeaderLength));
            }
            Array.Resize(ref _Buffer, length);

            var bodyLength = length - MinHeaderLength;
            if (stream.Read(_Buffer, MinHeaderLength, bodyLength) != bodyLength)
                throw new EndOfStreamException();

            FileType = (FileType)BitConverter.ToInt32(_Buffer, FileTypeOffset);
            Length = BitConverter.ToInt32(_Buffer, FileLengthOffset) << FileLengthShift;
            EndOfHeader = stream.Position;
        }

        #endregion
    }
}
