using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Imaging {
    using IO;

    /// <summary>
    /// Image file stored inside SqPack.
    /// </summary>
    public class ImageFile : File {
        #region Fields
        private ImageHeader _ImageHeader;
        private WeakReference<byte[]> _BufferCache = null;
        private WeakReference<System.Drawing.Image> _ImageCache = null;
        #endregion

        #region Properties
        public ImageHeader ImageHeader { get { return _ImageHeader; } }
        public int Width { get { return ImageHeader.Width; } }
        public int Height { get { return ImageHeader.Height; } }
        public ImageFormat Format { get { return ImageHeader.Format; } }
        #endregion

        #region Constructor
        public ImageFile(Directory directory, FileCommonHeader commonHeader)
            : base(directory, commonHeader) {

            var stream = GetSourceStream();
            stream.Position = CommonHeader.EndOfHeader;
            _ImageHeader = new ImageHeader(stream);
        }
        #endregion

        #region Read
        public System.Drawing.Image GetImage() {
            System.Drawing.Image image;

            if (_ImageCache == null || !_ImageCache.TryGetTarget(out image)) {
                image = ImageConverter.Convert(this);

                if (_ImageCache == null)
                    _ImageCache = new WeakReference<System.Drawing.Image>(image);
                else
                    _ImageCache.SetTarget(image);
            }

            return image;
        }
        public override byte[] GetData() {
            byte[] buffer;

            if (_BufferCache == null || !_BufferCache.TryGetTarget(out buffer)) {
                buffer = Read();

                if (_BufferCache == null)
                    _BufferCache = new WeakReference<byte[]>(buffer);
                else
                    _BufferCache.SetTarget(buffer);
            }

            return buffer;
        }
        private byte[] Read() {
            var sourceStream = GetSourceStream();
            var offsets = GetBlockOffsets();

            byte[] data;
            using (var dataStream = new MemoryStream((int)CommonHeader.Length)) {
                foreach (var offset in offsets) {
                    sourceStream.Position = ImageHeader.EndOfHeader + offset;
                    ReadBlock(sourceStream, dataStream);
                }
                data = dataStream.ToArray();
            }
            return data;
        }
        private int[] GetBlockOffsets() {
            const int CountOffset = 0x14;
            const int EntryLength = 0x14;
            const int BlockInfoOffset = 0x18;

            var count = BitConverter.ToInt16(CommonHeader._Buffer, CountOffset);
            int currentOffset = 0;
            var offsets = new List<int>();

            for (int i = BlockInfoOffset + count * EntryLength; i + 2 <= CommonHeader._Buffer.Length; i += 2) {
                var len = BitConverter.ToUInt16(CommonHeader._Buffer, i);
                if (len == 0)
                    break;
                offsets.Add(currentOffset);
                currentOffset += len;
            }

            return offsets.ToArray();
        }
        #endregion
    }
}
