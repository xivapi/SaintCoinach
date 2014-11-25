using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    partial class Directory : IEnumerable<File> {
        #region Enumerator
        private class Enumerator : IEnumerator<File> {
            #region Fields
            private Directory _Directory;
            private File _Current = null;
            private IEnumerator<IndexFile> _InnerEnumerator;
            #endregion

            #region Constructor
            public Enumerator(Directory dir) {
                _Directory = dir;
                _InnerEnumerator = dir.Index.Files.Values.GetEnumerator();
            }
            #endregion

            #region IEnumerator<SqPackDirectory> Members

            public File Current {
                get { return _Current; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {

            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current {
                get { return Current; }
            }

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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
