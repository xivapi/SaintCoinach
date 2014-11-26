using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    /// <summary>
    /// Base class for files inside the SqPack.
    /// </summary>
    public abstract class File {
        #region Fields
        private Directory _Directory;
        private FileCommonHeader _CommonHeader;
        private string _Name;
        #endregion

        #region Properties
        public Directory Directory { get { return _Directory; } }
        public FileCommonHeader CommonHeader { get { return _CommonHeader; } }
        public IndexFile Index { get { return CommonHeader.Index; } }
        public string Name {
            get { return _Name ?? Index.FileKey.ToString("X8"); }
            internal set { _Name = value; }
        }
        public string Path {
            get { return string.Join("/", Directory.Path, this.Name); }
        }
        #endregion

        #region Constructor
        protected File(Directory directory, FileCommonHeader commonHeader) {
            _Directory = directory;
            _CommonHeader = commonHeader;
        }
        #endregion

        #region Abstracts
        public abstract byte[] GetData();
        public virtual Stream GetStream() {
            return new MemoryStream(GetData());
        }
        #endregion

        #region Helpers
        protected Stream GetSourceStream() {
            return _Directory.Pack.GetDataStream(Index.DatFile);
        }

        protected static byte[] ReadBlock(Stream stream) {
            byte[] block;
            using (var msOut = new MemoryStream()) {
                ReadBlock(stream, msOut);
                block = msOut.ToArray();
            }
            return block;
        }
        protected static void ReadBlock(Stream inStream, Stream outStream) {
            const uint Magic = 0x00000010;

            const int HeaderLength = 0x10;
            const int MagicOffset = 0x00;
            const int SourceSizeOffset = 0x08;
            const int RawSizeOffset = 0x0C;

            const int BlockPadding = 0x80;

            const int CompressionThreshold = 0x7D00;

            /*
             * Block:
             * 10h  Header
             * *    Data
             * 
             * Header:
             * 4h   Magic
             * 4h   Unknown / Zero
             * 4h   Size in source
             * 4h   Raw size
             * -> If size in source >= 7D00h then data is uncompressed
             */

            var header = new byte[HeaderLength];
            if (inStream.Read(header, 0, HeaderLength) != HeaderLength)
                throw new EndOfStreamException();

            var magicCheck = BitConverter.ToInt32(header, MagicOffset);
            var sourceSize = BitConverter.ToInt32(header, SourceSizeOffset);
            var rawSize = BitConverter.ToInt32(header, RawSizeOffset);

            if (magicCheck != Magic)
                throw new NotSupportedException("Magic number not present (-> don't know how to continue).");

            var isCompressed = sourceSize < CompressionThreshold;

            int blockSize;
            if (isCompressed)
                blockSize = sourceSize;
            else
                blockSize = rawSize;

            if ((blockSize + HeaderLength) % BlockPadding != 0)
                blockSize += BlockPadding - ((blockSize + HeaderLength) % BlockPadding);    // Add padding if necessary

            var buffer = new byte[blockSize];
            if (inStream.Read(buffer, 0, blockSize) != blockSize)
                throw new EndOfStreamException();

            if (isCompressed) {
                var currentPosition = outStream.Position;
                Inflate(buffer, outStream);
                var dLen = outStream.Position - currentPosition;
                if (dLen != rawSize)
                    throw new InvalidDataException("Inflated block does not match indicated size.");
            } else {
                outStream.Write(buffer, 0, buffer.Length);
            }
        }
        private static void Inflate(byte[] buffer, Stream outStream) {
            using (var ms = new MemoryStream(buffer)) {
                using (var deflate = new DeflateStream(ms, CompressionMode.Decompress)) {
                    deflate.CopyTo(outStream);
                }
            }
        }
        #endregion

        #region Hashcode / Equals
        public override int GetHashCode() {
            return Path.GetHashCode();
        }
        public override bool Equals(object obj) {
            var asFile = obj as File;
            if (asFile != null)
                return (asFile.Index.FileKey == this.Index.FileKey
                    && asFile.Index.DirectoryKey == this.Index.DirectoryKey
                    && asFile.Index.DatFile == this.Index.DatFile);
            return false;
        }
        #endregion

        public override string ToString() {
            return Path;
        }
    }
}
