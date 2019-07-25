using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SaintCoinach.IO {
    /// <summary>
    ///     Directory inside a SqPack.
    /// </summary>
    public partial class Directory : IPackSource {
        #region Fields

        private readonly Dictionary<string, uint> _FileNameMap = new Dictionary<string, uint>();
        private readonly ConcurrentDictionary<uint, WeakReference<File>> _Files = new ConcurrentDictionary<uint, WeakReference<File>>();
        private string _Path;

        #endregion

        #region Properties

        public Pack Pack { get; private set; }
        public IndexDirectory Index { get; private set; }

        public string Path {
            get { return _Path ?? string.Join("/", Pack.ToString(), Index.Key.ToString("X8")); }
            internal set { _Path = value; }
        }

        #endregion

        #region Constructors

        #region Constructor

        public Directory(Pack pack, IndexDirectory index) {
            Pack = pack;
            Index = index;
        }

        #endregion

        #endregion

        public override string ToString() {
            return Path;
        }

        #region Get

        public bool FileExists(string name) {
            uint hash;/*
            if (!_FileNameMap.TryGetValue(name, out hash))
                _FileNameMap.Add(name, hash = Hash.Compute(name));*/
            hash = Hash.Compute(name);
            return FileExists(hash);
        }

        public bool FileExists(uint fileKey) {
            return Index.Files.ContainsKey(fileKey);
        }

        public File GetFile(string name) {
            uint hash;/*
            if (!_FileNameMap.TryGetValue(name, out hash))
                _FileNameMap.Add(name, hash = Hash.Compute(name));*/
            hash = Hash.Compute(name);

            var file = GetFile(hash);
            if (file == null)
                throw new System.IO.FileNotFoundException("Pack file not found '" + name + "'");
            file.Path = string.Format("{0}/{1}", this.Path, name);
            return file;
        }

        public File GetFile(uint key) {
            if (_Files.TryGetValue(key, out var fileRef) && fileRef.TryGetTarget(out var file))
                return file;

            if (!Index.Files.TryGetValue(key, out var index))
                return null;

            file = FileFactory.Get(this.Pack, index);
            _Files.AddOrUpdate(key,
                k => new WeakReference<File>(file),
                (k, r) => {
                    r.SetTarget(file);
                    return r;
                });
            return file;
        }

        public bool TryGetFile(string name, out File file) {
            uint hash;/*
            if (!_FileNameMap.TryGetValue(name, out hash))
                _FileNameMap.Add(name, hash = Hash.Compute(name));*/
            hash = Hash.Compute(name);

            var result = TryGetFile(hash, out file);
            if (result)
                file.Path = string.Format("{0}/{1}", this.Path, name);
            return result;
        }

        public bool TryGetFile(uint key, out File file) {
            if (_Files.TryGetValue(key, out var fileRef) && fileRef.TryGetTarget(out file))
                return true;

            if (Index.Files.TryGetValue(key, out var index)) {
                var theFile = FileFactory.Get(this.Pack, index);
                _Files.AddOrUpdate(key,
                    k => new WeakReference<File>(theFile),
                    (k, r) => {
                        r.SetTarget(theFile);
                        return r;
                    });
                file = theFile;
                return true;
            }

            file = null;
            return false;
        }

        #endregion
    }
}
