using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    /// <summary>
    /// Directory-entry inside an index file.
    /// </summary>
    public class IndexDirectory {
        #region Fields
        private uint _Key;
        private int _Offset;
        private int _Count;
        private IReadOnlyDictionary<uint, IndexFile> _Files;
        #endregion

        #region Properties
        public uint Key { get { return _Key; } }
        public int Offset { get { return _Offset; } }
        public int Count { get { return _Count; } }
        public IReadOnlyDictionary<uint, IndexFile> Files { get { return _Files; } }
        #endregion

        #region Constructor
        public IndexDirectory(BinaryReader reader) {
            ReadMeta(reader);
            var pos = reader.BaseStream.Position;
            ReadFiles(reader);
            reader.BaseStream.Position = pos;
        }
        #endregion

        #region Build
        private void ReadMeta(BinaryReader reader) {
            _Key = reader.ReadUInt32();
            _Offset = reader.ReadInt32();
            var len = reader.ReadInt32();
            _Count = len / 0x10;

            reader.ReadInt32(); // Zero
        }
        private void ReadFiles(BinaryReader reader) {
            reader.BaseStream.Position = Offset;

            var rem = Count;
            var files = new List<IndexFile>();
            while (rem-- > 0)
                files.Add(new IndexFile(reader));

            _Files = new ReadOnlyDictionary<uint, IndexFile>(files.ToDictionary(_ => _.FileKey, _ => _));
        }
        #endregion
    }
}
