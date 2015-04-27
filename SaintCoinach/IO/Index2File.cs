using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    public class Index2File : IIndexFile {
        #region Properties

        public uint FileKey { get; private set; }
        public int Offset { get; private set; }

        /// <summary>
        ///     In which .dat* file the data is located.
        /// </summary>
        public byte DatFile { get; private set; }

        #endregion
        
        #region Constructor

        public Index2File(BinaryReader reader) {
            FileKey = reader.ReadUInt32();

            var baseOffset = reader.ReadInt32();
            DatFile = (byte)((baseOffset & 0x7) >> 1);
            Offset = (int)((baseOffset & 0xFFFFFFF8) << 3);
        }

        #endregion
    }
}
