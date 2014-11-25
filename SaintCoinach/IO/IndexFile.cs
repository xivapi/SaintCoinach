using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    /// <summary>
    /// File entry inside an index file.
    /// </summary>
    public class IndexFile {
        #region Fields
        private uint _FileKey;
        private int _DirectoryKey;
        private int _Offset;
        private byte _DatFile;
        #endregion

        #region Properties
        public uint FileKey { get { return _FileKey; } }
        public int DirectoryKey { get { return _DirectoryKey; } }
        public int Offset { get { return _Offset; } }
        /// <summary>
        /// In which .dat* file the data is located.
        /// </summary>
        public byte DatFile { get { return _DatFile; } }
        #endregion

        #region Constructor
        public IndexFile(BinaryReader reader) {
            _FileKey = reader.ReadUInt32();
            _DirectoryKey = reader.ReadInt32();

            var baseOffset = reader.ReadInt32();
            _DatFile = (byte)((baseOffset & 0x7) >> 1);
            _Offset = (int)((baseOffset & 0xFFFFFFF8) << 3);

            reader.ReadInt32(); // Zero
        }
        #endregion
    }
}
