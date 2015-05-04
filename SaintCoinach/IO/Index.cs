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
    ///     Class representing the data inside a *.index file.
    /// </summary>
    public class Index {
        #region Properties

        public PackIdentifier PackId { get; private set; }
        public IndexHeader Header { get; private set; }
        public IReadOnlyDictionary<uint, IndexDirectory> Directories { get; private set; }

        #endregion

        #region Constructor

        public Index(PackIdentifier packId, string path) {
            PackId = packId;

            using (var file = IOFile.OpenRead(path)) {
                using (var reader = new BinaryReader(file))
                    Build(reader);
            }
        }

        public Index(PackIdentifier packId, Stream stream) {
            PackId = packId;

            using (var reader = new BinaryReader(stream, Encoding.Default, true))
                Build(reader);
        }

        public Index(PackIdentifier packId, BinaryReader reader) {
            PackId = packId;

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
            ReadDirectories(reader);
        }

        private void ReadHeader(BinaryReader reader) {
            const int HeaderOffsetOffset = 0x0C;

            reader.BaseStream.Position = HeaderOffsetOffset;
            var headerOffset = reader.ReadInt32();

            reader.BaseStream.Position = headerOffset;
            Header = new IndexHeader(reader);
        }

        private void ReadDirectories(BinaryReader reader) {
            reader.BaseStream.Position = Header.DirectoriesOffset;

            var rem = Header.DirectoriesCount;
            var dirs = new List<IndexDirectory>();
            while (rem-- > 0)
                dirs.Add(new IndexDirectory(PackId, reader));

            Directories = new ReadOnlyDictionary<uint, IndexDirectory>(dirs.ToDictionary(_ => _.Key, _ => _));
        }

        #endregion
    }
}
