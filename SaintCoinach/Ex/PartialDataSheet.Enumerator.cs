using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    partial class PartialDataSheet<T> {
        private class Enumerator : IEnumerator<T> {
            #region Fields
            private PartialDataSheet<T> _Sheet;
            private IEnumerator<KeyValuePair<int, int>> _InnerEnumerator;
            #endregion

            #region Constructor
            public Enumerator(PartialDataSheet<T> sheet) {
                _Sheet = sheet;
                _InnerEnumerator = sheet._RowOffsets.GetEnumerator();
            }
            #endregion

            #region IEnumerator<T> Members

            public T Current {
                get {
                    var inner = _InnerEnumerator.Current;
                    return _Sheet._Rows.GetOrAdd(inner.Key, k => _Sheet.CreateRow(k, inner.Value));
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {
                _InnerEnumerator.Dispose();
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current {
                get { return Current; }
            }

            public bool MoveNext() {
                return _InnerEnumerator.MoveNext();
            }

            public void Reset() {
                _InnerEnumerator.Reset();
            }

            #endregion
        }
    }
}
