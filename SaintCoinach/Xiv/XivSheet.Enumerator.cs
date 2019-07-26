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
                    return _Sheet._Rows.GetOrAdd(srcRow.Key, k => _Sheet.CreateRow(srcRow));
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
