using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    partial class XivSheet<T> {
        private class Enumerator : IEnumerator<T> {
            #region Fields
            private XivSheet<T> _Sheet;
            private IEnumerator _SourceEnumerator;
            #endregion

            #region Constructors
            public Enumerator(XivSheet<T> sheet) {
                _Sheet = sheet;
                _SourceEnumerator = sheet._Source.GetEnumerator();
            }
            #endregion

            #region IEnumerator<T> Members

            public T Current {
                get {
                    var srcRow = (Ex.Relational.IRelationalRow)_SourceEnumerator.Current;
                    var key = srcRow.Key;

                    T row;
                    if (_Sheet._Rows.TryGetValue(key, out row)) return row;

                    row = _Sheet.CreateRow(srcRow);
                    _Sheet._Rows.Add(key, row);
                    return row;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {
                
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current {
                get { return Current; }
            }

            public bool MoveNext() {
                return _SourceEnumerator.MoveNext();
            }

            public void Reset() {
                _SourceEnumerator.Reset();
            }

            #endregion
        }
    }
}
