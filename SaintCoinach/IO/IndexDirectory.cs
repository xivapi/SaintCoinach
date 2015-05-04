using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SaintCoinach.IO {
    /// <summary>
    ///     Directory-entry inside an index file.
    /// </summary>
    public class IndexDirectory {
        #region Properties

        public PackIdentifier PackId { get; private set; }
        public uint Key { get; private set; }
        public int Offset { get; private set; }
        public int Count { get; private set; }
        public IReadOnlyDictionary<uint, IndexFile> Files { get; private set; }

        #endregion

        #region Constructors

        public IndexDirectory(PackIdentifier packId, BinaryReader reader) {
            PackId = packId;

            ReadMeta(reader);
            var pos = reader.BaseStream.Position;
            ReadFiles(reader);
            reader.BaseStream.Position = pos;
        }

        #endregion

        #region Build

        private void ReadMeta(BinaryReader reader) {
            Key = reader.ReadUInt32();
            Offset = reader.ReadInt32();
            var len = reader.ReadInt32();
            Count = len / 0x10;

            reader.ReadInt32(); // Zero
        }

        private void ReadFiles(BinaryReader reader) {
            reader.BaseStream.Position = Offset;

            var rem = Count;
            var files = new List<IndexFile>();
            while (rem-- > 0)
                files.Add(new IndexFile(PackId, reader));

            Files = new ReadOnlyDictionary<uint, IndexFile>(files.ToDictionary(_ => _.FileKey, _ => _));
        }

        #endregion
    }
}
