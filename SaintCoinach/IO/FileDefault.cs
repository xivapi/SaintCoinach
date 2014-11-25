using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    /// <summary>
    /// A file inside SqPack without special attributes.
    /// </summary>
    public class FileDefault : File {
        #region Fields
        private WeakReference<byte[]> _BufferCache = null;
        #endregion

        #region Constructor
        public FileDefault(Directory directory, FileCommonHeader header) : base(directory, header) { }
        #endregion

        #region Read
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
            const int BlockCountOffset = 0x14;
            const int BlockInfoOffset = 0x18;
            const int BlockInfoLength = 0x08;

            var sourceStream = GetSourceStream();
            var blockCount = BitConverter.ToInt16(CommonHeader._Buffer, BlockCountOffset);

            byte[] data;
            using (var dataStream = new MemoryStream((int)CommonHeader.Length)) {
                for (var i = 0; i < blockCount; ++i) {
                    var blockOffset = BitConverter.ToInt32(CommonHeader._Buffer, BlockInfoOffset + i * BlockInfoLength);
                    sourceStream.Position = CommonHeader.EndOfHeader + blockOffset;
                    ReadBlock(sourceStream, dataStream);
                }

                data = dataStream.ToArray();
            }

            return data;
        }
        #endregion
    }
}
