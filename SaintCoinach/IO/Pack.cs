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
    public partial class Pack : IEnumerable<File> {
        #region Static

        private const string IndexFileFormat = "{0:x2}0000.win32.index";
        private const string Index2FileFormat = "{0:x2}0000.win32.index2";
        private const string DatFileFormat = "{0:x2}0000.win32.dat{1}";

        #endregion

        #region Fields

        private readonly Dictionary<Tuple<Thread, byte>, WeakReference<Stream>> _DataStreams =
            new Dictionary<Tuple<Thread, byte>, WeakReference<Stream>>();

        private readonly string _Name;

        private bool _KeepInMemory = false;
        private Dictionary<int, byte[]> _Buffers = new Dictionary<int,byte[]>();

        #endregion

        #region Properties

        public PackCollection Collection { get; private set; }
        public DirectoryInfo DataDirectory { get; private set; }
        public IPackSource Source { get; private set; }
        public byte Key { get; private set; }
        public string Name { get { return _Name ?? Key.ToString("x2"); } }
        public bool KeepInMemory {
            get { return _KeepInMemory; }
            set {
                if (value == _KeepInMemory)
                    return;

                Stream t;
                if (_DataStreams.Any(i => i.Value.TryGetTarget(out t)))
                    throw new InvalidOperationException();
                _DataStreams.Clear();

                _KeepInMemory = value;
                if (!value)
                    _Buffers.Clear();
            }
        }

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


            if (KeepInMemory) {
                if (!_Buffers.ContainsKey(datFile)) {
                    _Buffers.Add(datFile, IOFile.ReadAllBytes(fullPath));
                }
                stream = new MemoryStream(_Buffers[datFile], false);
            } else
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
            new Dictionary<string, byte> {
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
            var index2Path = Path.Combine(DataDirectory.FullName, string.Format(Index2FileFormat, Key));
            if (IOFile.Exists(indexPath))
                Source = new IndexSource(this, new Index(indexPath));
            else if (IOFile.Exists(index2Path))
                Source = new Index2Source(this, new Index2(index2Path));
            else
                throw new FileNotFoundException();
        }

        #endregion

        #region Fields

        public bool FileExists(string path) {
            return Source.FileExists(path);
        }
        public bool TryGetFile(string path, out File value) {
            return Source.TryGetFile(path, out value);
        }
        public File GetFile(string path) {
            return Source.GetFile(path);
        }

        #endregion

        #region IEnumerable<File> Members

        public IEnumerator<File> GetEnumerator() {
            return Source.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
