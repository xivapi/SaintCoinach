using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    /// <summary>
    /// Header of an index file.
    /// </summary>
    public struct IndexHeader {
        public readonly int FilesOffset;
        public readonly int FilesCount;

        public readonly int DirectoriesOffset;
        public readonly int DirectoriesCount;

        public IndexHeader(BinaryReader reader) {
            const int FileDataOffset = 0x08;
            const int DirectoryDataOffset = 0xE4;

            var start = reader.BaseStream.Position;

            reader.BaseStream.Position = start + FileDataOffset;
            FilesOffset = reader.ReadInt32();
            var filesLength = reader.ReadInt32();
            FilesCount = filesLength / 0x10;

            reader.BaseStream.Position = start + DirectoryDataOffset;
            DirectoriesOffset = reader.ReadInt32();
            var dirLength = reader.ReadInt32();
            DirectoriesCount = dirLength / 0x10;
        }
    }
}
