using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    /// <summary>
    ///     Header of an index2 file.
    /// </summary>
    public class Index2Header {
        #region Fields

        private readonly int _FilesCount;
        private readonly int _FilesOffset;

        #endregion

        #region Properties

        public int FileCount { get { return _FilesCount; } }
        public int FilesOffset { get { return _FilesOffset; } }

        #endregion

        #region Constructors

        public Index2Header(BinaryReader reader) {
            const int FileDataOffset = 0x08;

            var start = reader.BaseStream.Position;

            reader.BaseStream.Position = start + FileDataOffset;
            _FilesOffset = reader.ReadInt32();
            var filesLength = reader.ReadInt32();
            _FilesCount = filesLength / 0x08;
        }

        #endregion
    }
}
