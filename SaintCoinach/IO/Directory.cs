using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    /// <summary>
    /// Directory inside a SqPack.
    /// </summary>
    public partial class Directory {
        #region Fields
        private Pack _Pack;
        private IndexDirectory _Index;
        private string _Path;
        private Dictionary<uint, WeakReference<File>> _Files = new Dictionary<uint, WeakReference<File>>();
        private Dictionary<string, uint> _FileNameMap = new Dictionary<string, uint>();
        #endregion

        #region Properties
        public Pack Pack { get { return _Pack; } }
        public IndexDirectory Index { get { return _Index; } }
        public string Path {
            get { return _Path ?? string.Join("/", Pack.Name, Index.Key.ToString("X8")); }
            internal set { _Path = value; }
        }
        #endregion

        #region Constructor
        public Directory(Pack pack, IndexDirectory index) {
            _Pack = pack;
            _Index = index;
        }
        #endregion

        #region Get
        public bool FileExists(string name) {
            uint hash;
            if (!_FileNameMap.TryGetValue(name, out hash))
                _FileNameMap.Add(name, hash = Hash.Compute(name));
            return FileExists(hash);
        }
        public bool FileExists(uint fileKey) {
            return Index.Files.ContainsKey(fileKey);
        }
        public File GetFile(string name) {
            uint hash;
            if (!_FileNameMap.TryGetValue(name, out hash))
                _FileNameMap.Add(name, hash = Hash.Compute(name));

            var file = GetFile(hash);
            file.Name = name;
            return file;
        }
        public File GetFile(uint key) {
            WeakReference<File> fileRef;
            File file;
            if (_Files.TryGetValue(key, out fileRef) && fileRef.TryGetTarget(out file))
                return file;

            var index = Index.Files[key];
            file = FileFactory.Get(this, index);
            if (_Files.ContainsKey(key))
                _Files[key].SetTarget(file);
            else
                _Files.Add(key, new WeakReference<File>(file));
            return file;
        }
        public bool TryGetFile(string name, out File file) {
            uint hash;
            if (!_FileNameMap.TryGetValue(name, out hash))
                _FileNameMap.Add(name, hash = Hash.Compute(name));

            var result = TryGetFile(hash, out file);
            if (result)
                file.Name = name;
            return result;
        }
        public bool TryGetFile(uint key, out File file) {
            WeakReference<File> fileRef;
            if (_Files.TryGetValue(key, out fileRef) && fileRef.TryGetTarget(out file))
                return true;

            IndexFile index;
            if (Index.Files.TryGetValue(key, out index)) {
                file = FileFactory.Get(this, index);
                if (_Files.ContainsKey(key))
                    _Files[key].SetTarget(file);
                else
                    _Files.Add(key, new WeakReference<File>(file));
                return true;
            }

            file = null;
            return false;
        }
        #endregion

        public override string ToString() {
            return Path;
        }
    }
}
