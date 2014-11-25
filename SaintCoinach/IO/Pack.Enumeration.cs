using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    partial class Pack : IEnumerable<Directory> {
        #region Enumerator
        private class Enumerator : IEnumerator<Directory> {
            #region Fields
            private Pack _Pack;
            private Directory _Current = null;
            private IEnumerator<IndexDirectory> _InnerEnumerator;
            #endregion

            #region Constructor
            public Enumerator(Pack pack) {
                _Pack = pack;
                _InnerEnumerator = pack.Index.Directories.Values.GetEnumerator();
            }
            #endregion

            #region IEnumerator<SqPackDirectory> Members

            public Directory Current {
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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
