using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    public class Index2File : IIndexFile {
        #region Properties

        public PackIdentifier PackId { get; private set; }
        public uint FileKey { get; private set; }
        public long Offset { get; private set; }

        /// <summary>
        ///     In which .dat* file the data is located.
        /// </summary>
        public byte DatFile { get; private set; }

        #endregion
        
        #region Constructor

        public Index2File(PackIdentifier packId, BinaryReader reader) {
            PackId = packId;
            FileKey = reader.ReadUInt32();

            var baseOffset = reader.ReadUInt32();
            // Index2 stores the same low-nibble flags layout as Index:
            // bit0 = flag, bits1-3 = dat file id, upper bits = offset/8.
            DatFile = (byte)((baseOffset & 0x000F) >> 1);
            Offset = ((long)(baseOffset & 0xFFFFFFF0u)) * 0x08L;
        }

        #endregion

        #region IEquatable<IIndexFile> Members

        public override int GetHashCode() {
            return (int)(((DatFile << 24) | PackId.GetHashCode()) ^ Offset.GetHashCode());
        }
        public override bool Equals(object obj) {
            if (obj is IIndexFile)
                return Equals((IIndexFile)obj);
            return false;
        }
        public bool Equals(IIndexFile other) {
            return other.PackId.Equals(this.PackId)
                && other.DatFile == this.DatFile
                && other.Offset == this.Offset;
        }

        #endregion
    }
}
