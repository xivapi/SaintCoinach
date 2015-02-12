using System.Collections;
using System.Collections.Generic;

namespace SaintCoinach.IO {
    partial class Directory : IEnumerable<File> {
        #region Enumerator

        private class Enumerator : IEnumerator<File> {
            #region Fields

            private readonly Directory _Directory;
            private readonly IEnumerator<IndexFile> _InnerEnumerator;
            private File _Current;

            #endregion

            #region Constructors

            #region Constructor

            public Enumerator(Directory dir) {
                _Directory = dir;
                _InnerEnumerator = dir.Index.Files.Values.GetEnumerator();
            }

            #endregion

            #endregion

            #region IEnumerator<SqPackDirectory> Members

            public File Current { get { return _Current; } }

            #endregion

            #region IDisposable Members

            public void Dispose() { }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current { get { return Current; } }

            public bool MoveNext() {
                var res = _InnerEnumerator.MoveNext();
                if (res)
                    _Directory.TryGetFile(_InnerEnumerator.Current.FileKey, out _Current);
                else
                    _Current = null;
                return res;
            }

            public void Reset() {
                _InnerEnumerator.Reset();
                _Current = null;
            }

            #endregion
        }

        #endregion

        #region IEnumerable<SqPackFile> Members

        public IEnumerator<File> GetEnumerator() {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
