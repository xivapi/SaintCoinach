using System.Collections;
using System.Collections.Generic;

namespace SaintCoinach.IO {
    partial class Pack : IEnumerable<Directory> {
        #region Enumerator

        private class Enumerator : IEnumerator<Directory> {
            #region Fields

            private readonly IEnumerator<IndexDirectory> _InnerEnumerator;
            private readonly Pack _Pack;
            private Directory _Current;

            #endregion

            #region Constructors

            #region Constructor

            public Enumerator(Pack pack) {
                _Pack = pack;
                _InnerEnumerator = pack.Index.Directories.Values.GetEnumerator();
            }

            #endregion

            #endregion

            #region IEnumerator<SqPackDirectory> Members

            public Directory Current { get { return _Current; } }

            #endregion

            #region IDisposable Members

            public void Dispose() { }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current { get { return Current; } }

            public bool MoveNext() {
                var res = _InnerEnumerator.MoveNext();
                if (res)
                    _Pack.TryGetDirectory(_InnerEnumerator.Current.Key, out _Current);
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

        #region IEnumerable<SqPackDirectory> Members

        public IEnumerator<Directory> GetEnumerator() {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
