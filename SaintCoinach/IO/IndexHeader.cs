using System.IO;

namespace SaintCoinach.IO {
    /// <summary>
    ///     Header of an index file.
    /// </summary>
    public struct IndexHeader {
        #region Fields

        private readonly int _DirectoriesCount;
        private readonly int _DirectoriesOffset;
        private readonly int _FilesCount;
        private readonly int _FilesOffset;

        #endregion

        #region Properties

        public int DirectoriesCount { get { return _DirectoriesCount; } }
        public int DirectoriesOffset { get { return _DirectoriesOffset; } }
        public int FilesCount { get { return _FilesCount; } }
        public int FilesOffset { get { return _FilesOffset; } }

        #endregion

        #region Constructors

        public IndexHeader(BinaryReader reader) {
            const int FileDataOffset = 0x08;
            const int DirectoryDataOffset = 0xE4;

            var start = reader.BaseStream.Position;

            reader.BaseStream.Position = start + FileDataOffset;
            _FilesOffset = reader.ReadInt32();
            var filesLength = reader.ReadInt32();
            _FilesCount = filesLength / 0x10;

            reader.BaseStream.Position = start + DirectoryDataOffset;
            _DirectoriesOffset = reader.ReadInt32();
            var dirLength = reader.ReadInt32();
            _DirectoriesCount = dirLength / 0x10;
        }

        #endregion
    }
}
