using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace SaintCoinach.IO {
    public class PackCollection {
        #region Fields

        private readonly ConcurrentDictionary<PackIdentifier, Pack> _Packs = new ConcurrentDictionary<PackIdentifier, Pack>();

        #endregion

        #region Properties

        public DirectoryInfo DataDirectory { get; private set; }

        public IEnumerable<Pack> Packs { get { return _Packs.Values; } }

        #endregion

        #region Constructor

        public PackCollection(string dataDirectory) : this(new DirectoryInfo(dataDirectory)) { }

        public PackCollection(DirectoryInfo dataDirectory) {
            if (dataDirectory == null)
                throw new ArgumentNullException("dataDirectory");
            if (!dataDirectory.Exists)
                throw new DirectoryNotFoundException();

            DataDirectory = dataDirectory;
        }

        #endregion

        #region Get

        public bool FileExists(string path) {
            return TryGetPack(path, out var pack) && pack.FileExists(path);
        }

        public File GetFile(string path) {
            var pack = GetPack(path);
            return pack.GetFile(path);
        }

        public Pack GetPack(PackIdentifier id) {
            return _Packs.GetOrAdd(id, i => new Pack(this, DataDirectory, id));
        }

        public Pack GetPack(string path) {
            var id = PackIdentifier.Get(path);
            return GetPack(id);
        }

        public bool TryGetFile(string path, out File file) {
            if (TryGetPack(path, out var pack))
                return pack.TryGetFile(path, out file);

            file = null;
            return false;
        }

        public bool TryGetPack(string path, out Pack pack) {
            pack = null;

            if (!PackIdentifier.TryGet(path, out var id))
                return false;

            pack =_Packs.GetOrAdd(id, i => new Pack(this, DataDirectory, id));
            return true;
        }

        #endregion
    }
}
