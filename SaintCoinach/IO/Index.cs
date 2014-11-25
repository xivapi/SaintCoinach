using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    using IOFile = System.IO.File;

    /// <summary>
    /// Class representing the data inside a *.index file.
    /// </summary>
    public class Index {
        #region Fields
        private IndexHeader _Header;
        private IReadOnlyDictionary<uint, IndexDirectory> _Directories;
        #endregion

        #region Properties
        public IndexHeader Header { get { return _Header; } }
        public IReadOnlyDictionary<uint, IndexDirectory> Directories { get { return _Directories; } }
        #endregion

        #region Constructor
        public Index(string path) {
            using (var file = IOFile.OpenRead(path)) {
                using (var reader = new BinaryReader(file))
                    Build(reader);
            }
        }
        public Index(Stream stream) {
            using (var reader = new BinaryReader(stream, Encoding.Default, true))
                Build(reader);
        }
        public Index(BinaryReader reader) {
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
            _Header = new IndexHeader(reader);
        }
        private void ReadDirectories(BinaryReader reader) {
            reader.BaseStream.Position = Header.DirectoriesOffset;

            var rem = Header.DirectoriesCount;
            var dirs = new List<IndexDirectory>();
            while (rem-- > 0)
                dirs.Add(new IndexDirectory(reader));

            _Directories = new ReadOnlyDictionary<uint, IndexDirectory>(dirs.ToDictionary(_ => _.Key, _ => _));
        }
        #endregion
    }
}
