using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SaintCoinach.IO {
    using IOFile = System.IO.File;

    /// <summary>
    ///     Class representing the data inside a *.index2 file.
    /// </summary>
    public class Index2 {
        #region Properties

        public Index2Header Header { get; private set; }
        public IReadOnlyDictionary<uint, Index2File> Files { get; private set; }
        public PackIdentifier PackId { get; private set; }

        #endregion
        
        #region Constructor

        public Index2(PackIdentifier packId, string path) {
            this.PackId = packId;

            using (var file = IOFile.OpenRead(path)) {
                using (var reader = new BinaryReader(file))
                    Build(reader);
            }
        }

        public Index2(PackIdentifier packId, Stream stream) {
            this.PackId = packId;

            using (var reader = new BinaryReader(stream, Encoding.Default, true))
                Build(reader);
        }

        public Index2(PackIdentifier packId, BinaryReader reader) {
            this.PackId = packId;

            Build(reader);
        }

        #endregion

        #region Build

        private void Build(BinaryReader reader) {
            const UInt64 SqPackMagic = 0x00006B6361507153;

            if (!reader.BaseStream.CanSeek)
                throw new NotSupportedException("Stream must be able to seek.");
            var fileMagic = reader.ReadUInt64();
            Debug.Assert(fileMagic == SqPackMagic, "Input stream is not a SqPack file.");

            ReadHeader(reader);
            ReadFiles(reader);
        }

        private void ReadHeader(BinaryReader reader) {
            const int HeaderOffsetOffset = 0x0C;

            reader.BaseStream.Position = HeaderOffsetOffset;
            var headerOffset = reader.ReadInt32();

            reader.BaseStream.Position = headerOffset;
            Header = new Index2Header(reader);
        }

        private void ReadFiles(BinaryReader reader) {
            reader.BaseStream.Position = Header.FilesOffset;

            var rem = Header.FileCount;
            var files = new List<Index2File>();
            while (rem-- > 0)
                files.Add(new Index2File(PackId, reader));

            Files = new ReadOnlyDictionary<uint, Index2File>(files.ToDictionary(_ => _.FileKey, _ => _));
        }

        #endregion
    }
}
