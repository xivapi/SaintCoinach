using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SaintCoinach.IO {
    using IOFile = System.IO.File;

    /// <summary>
    ///     Class for a SqPack.
    /// </summary>
    public partial class Pack {
        #region Static

        private const string IndexFileFormat = "{0:x2}0000.win32.index";
        private const string DatFileFormat = "{0:x2}0000.win32.dat{1}";

        #endregion

        #region Fields

        private readonly Dictionary<Tuple<Thread, byte>, WeakReference<Stream>> _DataStreams =
            new Dictionary<Tuple<Thread, byte>, WeakReference<Stream>>();

        private readonly Dictionary<uint, WeakReference<Directory>> _Directories =
            new Dictionary<uint, WeakReference<Directory>>();

        private readonly Dictionary<string, uint> _DirectoryPathMap = new Dictionary<string, uint>();
        private readonly string _Name;

        #endregion

        #region Properties

        public PackCollection Collection { get; private set; }
        public Index Index { get; private set; }
        public DirectoryInfo DataDirectory { get; private set; }
        public byte Key { get; private set; }
        public string Name { get { return _Name ?? Key.ToString("x2"); } }

        #endregion

        #region Helpers

        public Stream GetDataStream(byte datFile = 0) {
            var thread = Thread.CurrentThread;

            var key = Tuple.Create(thread, datFile);
            WeakReference<Stream> streamRef;
            Stream stream;
            lock (_DataStreams)
                _DataStreams.TryGetValue(key, out streamRef);

            if (streamRef == null || !streamRef.TryGetTarget(out stream))
                stream = null;

            if (stream != null) return stream;

            var baseName = String.Format(DatFileFormat, Key, datFile);
            var fullPath = Path.Combine(DataDirectory.FullName, baseName);

            stream = IOFile.OpenRead(fullPath);

            lock (_DataStreams) {
                if (_DataStreams.ContainsKey(key))
                    _DataStreams[key].SetTarget(stream);
                else
                    _DataStreams.Add(key, new WeakReference<Stream>(stream));
            }

            return stream;
        }

        #endregion

        public override string ToString() {
            return Name;
        }

        #region Name <> Key mapping

        private static readonly Dictionary<string, byte> RootToSqMap =
            new Dictionary<string, byte>(StringComparer.OrdinalIgnoreCase) {
                {
                    "common", 0x00
                }, {
                    "bgcommon", 0x01
                }, {
                    "bg", 0x02
                }, {
                    "cut", 0x03
                }, {
                    "chara", 0x04
                }, {
                    "shader", 0x05
                }, {
                    "ui", 0x06
                }, {
                    "sound", 0x07
                }, {
                    "vfx", 0x08
                }, {
                    "ui_script", 0x09
                }, {
                    "exd", 0x0a
                }, {
                    "game_script", 0x0b
                }, {
                    "music", 0x0c
                }, {
                    "_sqpack_test", 0x12
                }, {
                    "_debug", 0x13
                }
            };

        private static readonly Dictionary<byte, string> SqToRootMap = RootToSqMap.ToDictionary(_ => _.Value,
            _ => _.Key);

        private static string GetPathRoot(string path) {
            var search = path;
            var i = search.IndexOf('/');
            if (i >= 0)
                search = search.Substring(0, i);
            return search;
        }

        public static bool TryGetSqPackKey(string path, out byte key) {
            var search = GetPathRoot(path);
            return RootToSqMap.TryGetValue(search, out key);
        }

        public static byte GetSqPackKey(string path) {
            var search = GetPathRoot(path);
            return RootToSqMap[search];
        }

        public static bool TryGetSqPackName(byte key, out string name) {
            return SqToRootMap.TryGetValue(key, out name);
        }

        public static string GetSqPackName(byte key) {
            return SqToRootMap[key];
        }

        #endregion

        #region Constructor

        public Pack(string dataPath, byte key) : this(null, new DirectoryInfo(dataPath), key) { }
        public Pack(DirectoryInfo dataDirectory, byte key) : this(null, dataDirectory, key) { }

        public Pack(PackCollection collection, string dataPath, byte key)
            : this(collection, new DirectoryInfo(dataPath), key) { }

        public Pack(PackCollection collection, DirectoryInfo dataDirectory, byte key) {
            if (dataDirectory == null)
                throw new ArgumentNullException("dataDirectory");
            if (!dataDirectory.Exists)
                throw new DirectoryNotFoundException();

            Collection = collection;
            DataDirectory = dataDirectory;
            Key = key;
            TryGetSqPackName(key, out _Name);

            var indexPath = Path.Combine(DataDirectory.FullName, string.Format(IndexFileFormat, Key));
            Index = new Index(indexPath);
        }

        #endregion

        #region Get

        public bool DirectoryExists(string path) {
            uint hash;
            if (!_DirectoryPathMap.TryGetValue(path, out hash))
                _DirectoryPathMap.Add(path, hash = Hash.Compute(path));
            return DirectoryExists(hash);
        }

        public bool DirectoryExists(uint key) {
            return Index.Directories.ContainsKey(key);
        }

        public Directory GetDirectory(string path) {
            uint hash;
            if (!_DirectoryPathMap.TryGetValue(path, out hash))
                _DirectoryPathMap.Add(path, hash = Hash.Compute(path));

            var dir = GetDirectory(hash);
            dir.Path = path;
            return dir;
        }

        public Directory GetDirectory(uint key) {
            WeakReference<Directory> dirRef;
            Directory directory;
            if (_Directories.TryGetValue(key, out dirRef) && dirRef.TryGetTarget(out directory))
                return directory;

            var index = Index.Directories[key];
            directory = new Directory(this, index);
            if (_Directories.ContainsKey(key))
                _Directories[key].SetTarget(directory);
            else
                _Directories.Add(key, new WeakReference<Directory>(directory));
            return directory;
        }

        public bool TryGetDirectory(string path, out Directory directory) {
            uint hash;
            if (!_DirectoryPathMap.TryGetValue(path, out hash))
                _DirectoryPathMap.Add(path, hash = Hash.Compute(path));

            var result = TryGetDirectory(hash, out directory);
            if (result)
                directory.Path = path;
            return result;
        }

        public bool TryGetDirectory(uint key, out Directory directory) {
            WeakReference<Directory> dirRef;
            if (_Directories.TryGetValue(key, out dirRef) && dirRef.TryGetTarget(out directory))
                return true;

            IndexDirectory index;
            if (Index.Directories.TryGetValue(key, out index)) {
                directory = new Directory(this, index);
                if (_Directories.ContainsKey(key))
                    _Directories[key].SetTarget(directory);
                else
                    _Directories.Add(key, new WeakReference<Directory>(directory));
                return true;
            }

            directory = null;
            return false;
        }

        public bool FileExists(string path) {
            var lastSeperator = path.LastIndexOf('/');
            if (lastSeperator < 0)
                throw new ArgumentException();

            var dirPath = path.Substring(0, lastSeperator);
            var baseName = path.Substring(lastSeperator + 1);
            Directory dir;
            return TryGetDirectory(dirPath, out dir) && dir.FileExists(baseName);
        }

        public File GetFile(string path) {
            var lastSeperator = path.LastIndexOf('/');
            if (lastSeperator < 0)
                throw new ArgumentException();

            var dirPath = path.Substring(0, lastSeperator);
            var baseName = path.Substring(lastSeperator + 1);
            var dir = GetDirectory(dirPath);
            return dir.GetFile(baseName);
        }

        public File GetFile(uint directoryKey, uint fileKey) {
            var dir = GetDirectory(directoryKey);
            return dir.GetFile(fileKey);
        }

        public bool TryGetFile(string path, out File file) {
            var lastSeperator = path.LastIndexOf('/');
            if (lastSeperator < 0) {
                file = null;
                return false;
            }

            var dirPath = path.Substring(0, lastSeperator);
            var baseName = path.Substring(lastSeperator + 1);
            Directory dir;
            if (TryGetDirectory(dirPath, out dir))
                return dir.TryGetFile(baseName, out file);

            file = null;
            return false;
        }

        public bool TryGetFile(uint directoryKey, uint fileKey, out File file) {
            Directory dir;
            if (TryGetDirectory(directoryKey, out dir))
                return dir.TryGetFile(fileKey, out file);

            file = null;
            return false;
        }

        #endregion
    }
}
