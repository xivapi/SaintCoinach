using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    /// <summary>
    /// Shared header for a file inside SqPack.
    /// </summary>
    public class FileCommonHeader {
        #region Fields
        internal byte[] _Buffer;

        private IndexFile _Index;
        private FileType _FileType;
        private long _Length;
        private long _EndOfHeader;
        #endregion

        #region Properties
        public IndexFile Index { get { return _Index; } }
        public FileType FileType { get { return _FileType; } }
        public long Length { get { return _Length; } }
        public long EndOfHeader { get { return _EndOfHeader; } }
        public byte[] GetBuffer() {
            var b = new byte[_Buffer.Length];
            Array.Copy(_Buffer, b, b.Length);
            return b;
        }
        #endregion

        #region Constructor
        public FileCommonHeader(IndexFile index, Stream stream) {
            if (index == null)
                throw new ArgumentNullException("index");
            if (stream == null)
                throw new ArgumentNullException("stream");

            _Index = index;

            Read(stream);
        }
        #endregion

        #region Read
        private void Read(Stream stream) {
            const int FileTypeOffset = 0x04;
            const int FileLengthOffset = 0x10;
            const int FileLengthShift = 7;

            if (!stream.CanSeek)
                throw new NotSupportedException("Stream must be able to seek.");

            _Buffer = new byte[4];
            if (stream.Read(_Buffer, 0, 4) != 4)
                throw new EndOfStreamException();

            var length = BitConverter.ToInt32(_Buffer, 0);
            Array.Resize(ref _Buffer, length);

            var remaining = length - 4;
            if (stream.Read(_Buffer, 4, remaining) != remaining)
                throw new EndOfStreamException();

            _FileType = (FileType)BitConverter.ToInt32(_Buffer, FileTypeOffset);
            _Length = BitConverter.ToInt32(_Buffer, FileLengthOffset) << FileLengthShift;
            _EndOfHeader = stream.Position;
        }
        #endregion
    }
}
