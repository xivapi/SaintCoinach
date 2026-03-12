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
            const int MaxAllowedBlockBytes = 16 * 1024 * 1024;

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

            if (sourceSize <= 0 || rawSize <= 0)
                throw new InvalidDataException($"Invalid block size. Source={sourceSize}, Raw={rawSize}.");

            if (sourceSize > MaxAllowedBlockBytes || rawSize > MaxAllowedBlockBytes)
                throw new InvalidDataException($"Block too large. Source={sourceSize}, Raw={rawSize}, Limit={MaxAllowedBlockBytes}.");

            var isCompressed = sourceSize < CompressionThreshold;

            var blockSize = isCompressed ? sourceSize : rawSize;
            if (blockSize <= 0 || blockSize > MaxAllowedBlockBytes)
                throw new InvalidDataException($"Invalid block payload size {blockSize}. Limit={MaxAllowedBlockBytes}.");

            // An uncompressed block in an ScdOggFile was corrupted due to this
            // extra padding injecting extra 0s into the output stream.  I'm
            // not certain padding is only applied to compressed blocks.  The
            // padding algorithm may require more work if additional problems
            // are found.
            if (isCompressed && (blockSize + HeaderLength) % BlockPadding != 0)
                blockSize += BlockPadding - ((blockSize + HeaderLength) % BlockPadding); // Add padding if necessary

            var buffer = new byte[blockSize];
            if (inStream.Read(buffer, 0, blockSize) != blockSize)
                throw new EndOfStreamException();

            if (isCompressed) {
                var currentPosition = outStream.Position;
                Inflate(buffer, outStream, rawSize);
                var dLen = outStream.Position - currentPosition;
                if (dLen != rawSize)
                    throw new InvalidDataException("Inflated block does not match indicated size.");
            } else {
                outStream.Write(buffer, 0, buffer.Length);
            }
        }

        private static void Inflate(byte[] buffer, Stream outStream, int expectedSize) {
            if (expectedSize <= 0)
                throw new InvalidDataException($"Invalid expected inflated size {expectedSize}.");

            var temp = new byte[8192];
            var startPos = outStream.CanSeek ? outStream.Position : -1;

            void InflateFrom(Stream decompressor) {
                var total = 0;
                while (true) {
                    var read = decompressor.Read(temp, 0, temp.Length);
                    if (read <= 0)
                        break;

                    total += read;
                    if (total > expectedSize)
                        throw new InvalidDataException($"Inflated block exceeds expected size. Expected={expectedSize}, Actual>{total}.");

                    outStream.Write(temp, 0, read);
                }
            }

            bool LooksLikeZlibHeader(byte[] data) {
                if (data == null || data.Length < 2)
                    return false;

                var cmf = data[0];
                var flg = data[1];

                // RFC1950: compression method must be DEFLATE (8)
                if ((cmf & 0x0F) != 8)
                    return false;

                // RFC1950: window size CINFO <= 7
                if ((cmf >> 4) > 7)
                    return false;

                // RFC1950: header checksum (CMF*256 + FLG) % 31 == 0
                return ((cmf << 8) + flg) % 31 == 0;
            }

            // Pick decompressor by header to avoid flooding first-chance ZlibException in debugger.
            if (!LooksLikeZlibHeader(buffer)) {
                using (var compressedStream = new MemoryStream(buffer, false))
                using (var deflateStream = new Ionic.Zlib.DeflateStream(compressedStream, Ionic.Zlib.CompressionMode.Decompress, false)) {
                    InflateFrom(deflateStream);
                }
                return;
            }

            try {
                using (var compressedStream = new MemoryStream(buffer, false))
                using (var zlibStream = new Ionic.Zlib.ZlibStream(compressedStream, Ionic.Zlib.CompressionMode.Decompress, false)) {
                    InflateFrom(zlibStream);
                }
                return;
            } catch (Ionic.Zlib.ZlibException) {
                if (startPos >= 0)
                    outStream.Position = startPos;
            }

            try {
                using (var compressedStream = new MemoryStream(buffer, false))
                using (var deflateStream = new Ionic.Zlib.DeflateStream(compressedStream, Ionic.Zlib.CompressionMode.Decompress, false)) {
                    InflateFrom(deflateStream);
                }
            } catch (Exception ex) {
                throw new InvalidDataException("Unable to inflate block with zlib/deflate fallback.", ex);
            }
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
