using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    public class PackCollection {
        #region Fields
        private DirectoryInfo _DataDirectory;
        private Dictionary<byte, Pack> _Packs = new Dictionary<byte, Pack>();
        #endregion

        #region Properties
        public DirectoryInfo DataDirectory { get { return _DataDirectory; } }
        #endregion

        #region Constructor
        public PackCollection(string dataDirectory) : this(new DirectoryInfo(dataDirectory)) { }
        public PackCollection(DirectoryInfo dataDirectory) {
            if (dataDirectory == null)
                throw new ArgumentNullException("dataDirectory");
            if (!dataDirectory.Exists)
                throw new DirectoryNotFoundException();

            _DataDirectory = dataDirectory;
        }
        #endregion

        #region Get
        public bool FileExists(string path) {
            Pack pack;
            if (TryGetPack(path, out pack))
                return pack.FileExists(path);
            return false;
        }
        public File GetFile(string path) {
            var pack = GetPack(path);
            return pack.GetFile(path);
        }
        public Directory GetDirectory(string path) {
            var pack = GetPack(path);
            return pack.GetDirectory(path);
        }
        public Pack GetPack(byte key) {
            Pack pack;
            if (!_Packs.TryGetValue(key, out pack)) {
                pack = new Pack(this, DataDirectory, key);
                _Packs.Add(key, pack);
            }
            return pack;
        }
        public Pack GetPack(string path) {
            var key = Pack.GetSqPackKey(path);
            Pack pack;
            if (!_Packs.TryGetValue(key, out pack)) {
                pack = new Pack(this, DataDirectory, key);
                _Packs.Add(key, pack);
            }
            return pack;
        }
        public bool TryGetFile(string path, out File file) {
            Pack pack;
            if (TryGetPack(path, out pack))
                return pack.TryGetFile(path, out file);

            file = null;
            return false;
        }
        public bool TryGetDirectory(string path, out Directory dir) {
            Pack pack;
            if (TryGetPack(path, out pack))
                return pack.TryGetDirectory(path, out dir);

            dir = null;
            return false;
        }
        public bool TryGetPack(string path, out Pack pack) {
            pack = null;

            byte key;
            if (!Pack.TryGetSqPackKey(path, out key))
                return false;

            if (!_Packs.TryGetValue(key, out pack)) {
                pack = new Pack(this, DataDirectory, key);
                _Packs.Add(key, pack);
            }
            return true;
        }
        #endregion
    }
}
