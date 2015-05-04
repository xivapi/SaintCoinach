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

        private const string IndexFileFormat = "{0:x2}{1:x2}{2:x2}.win32.index";
        private const string Index2FileFormat = "{0:x2}{1:x2}{2:x2}.win32.index2";
        private const string DatFileFormat = "{0:x2}{1:x2}{2:x2}.win32.dat{3}";

        #endregion

        #region Fields

        private readonly Dictionary<Tuple<Thread, byte>, WeakReference<Stream>> _DataStreams =
            new Dictionary<Tuple<Thread, byte>, WeakReference<Stream>>();

        private bool _KeepInMemory = false;
        private Dictionary<int, byte[]> _Buffers = new Dictionary<int,byte[]>();

        #endregion

        #region Properties

        public PackIdentifier Id { get; private set; }
        public PackCollection Collection { get; private set; }
        public DirectoryInfo DataDirectory { get; private set; }
        public IPackSource Source { get; private set; }
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

            var baseName = String.Format(DatFileFormat, Id.TypeKey, Id.ExpansionKey, Id.Number, datFile);
            var fullPath = Path.Combine(DataDirectory.FullName, Id.Expansion, baseName);


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
            return string.Format("{0}/{1:x2}{2:x2}{3:x2}", Id.Expansion, Id.TypeKey, Id.ExpansionKey, Id.Number);
        }


        #region Constructor

        public Pack(string dataPath, PackIdentifier id) : this(null, new DirectoryInfo(dataPath), id) { }
        public Pack(DirectoryInfo dataDirectory, PackIdentifier id) : this(null, dataDirectory, id) { }

        public Pack(PackCollection collection, string dataPath, PackIdentifier id)
            : this(collection, new DirectoryInfo(dataPath), id) { }

        public Pack(PackCollection collection, DirectoryInfo dataDirectory, PackIdentifier id) {
            if (dataDirectory == null)
                throw new ArgumentNullException("dataDirectory");
            if (!dataDirectory.Exists)
                throw new DirectoryNotFoundException();

            Collection = collection;
            DataDirectory = dataDirectory;
            this.Id = id;

            var indexPath = Path.Combine(DataDirectory.FullName, id.Expansion, string.Format(IndexFileFormat, Id.TypeKey, Id.ExpansionKey, Id.Number));
            var index2Path = Path.Combine(DataDirectory.FullName, id.Expansion, string.Format(Index2FileFormat, Id.TypeKey, Id.ExpansionKey, Id.Number));
            if (IOFile.Exists(indexPath))
                Source = new IndexSource(this, new Index(id, indexPath));
            else if (IOFile.Exists(index2Path))
                Source = new Index2Source(this, new Index2(id, index2Path));
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
