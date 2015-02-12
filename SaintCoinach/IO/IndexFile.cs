using System.IO;

namespace SaintCoinach.IO {
    /// <summary>
    ///     File entry inside an index file.
    /// </summary>
    public class IndexFile {
        #region Properties

        public uint FileKey { get; private set; }
        public uint DirectoryKey { get; private set; }
        public int Offset { get; private set; }

        /// <summary>
        ///     In which .dat* file the data is located.
        /// </summary>
        public byte DatFile { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public IndexFile(BinaryReader reader) {
            FileKey = reader.ReadUInt32();
            DirectoryKey = reader.ReadUInt32();

            var baseOffset = reader.ReadInt32();
            DatFile = (byte)((baseOffset & 0x7) >> 1);
            Offset = (int)((baseOffset & 0xFFFFFFF8) << 3);

            reader.ReadInt32(); // Zero
        }

        #endregion

        #endregion
    }
}
