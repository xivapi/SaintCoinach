using System;
using System.IO;
using System.IO.Compression;

namespace SaintCoinach.IO {
    /// <summary>
    ///     Base class for files inside the SqPack.
    /// </summary>
    public abstract class File {
        #region Fields

        private string _Path;

        #endregion

        #region Properties

        public Pack Pack { get; private set; }
        public FileCommonHeader CommonHeader { get; private set; }
        public IIndexFile Index { get { return CommonHeader.Index; } }
        public string Path { get { return _Path ?? Index.FileKey.ToString("X8"); } internal set { _Path = value; } }

        #endregion

        #region Constructors

        protected File(Pack pack, FileCommonHeader commonHeader) {
            Pack = pack;
            CommonHeader = commonHeader;
        }

        #endregion

        public override string ToString() {
            return Path;
        }

        #region Abstracts

        public abstract byte[] GetData();

        public virtual Stream GetStream() {
            return new MemoryStream(GetData());
        }

        #endregion

        #region Helpers

        protected Stream GetSourceStream() {
            return this.Pack.GetDataStream(Index.DatFile);
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

            var blockSize = isCompressed ? sourceSize : rawSize;

            if ((blockSize + HeaderLength) % BlockPadding != 0)
                blockSize += BlockPadding - ((blockSize + HeaderLength) % BlockPadding); // Add padding if necessary

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
            var unc = Ionic.Zlib.DeflateStream.UncompressBuffer(buffer);
            outStream.Write(unc, 0, unc.Length);
        }

        #endregion

        #region Equals
        public override int GetHashCode() {
            return Index.GetHashCode();
        }
        public override bool Equals(object obj) {
            if (obj is File)
                return ((File)obj).Index.Equals(this.Index);
            return false;
        }
        #endregion
    }
}
