using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    partial class XivSheet2<T> {
        private class Enumerator : IEnumerator<T> {
            #region Fields
            private XivSheet2<T> _Sheet;
            private Ex.Variant2.DataRow _CurrentParent;
            private IEnumerator _SourceParentEnumerator;
            private IEnumerator _SourceSubEnumerator;
            #endregion

            #region Constructors
            public Enumerator(XivSheet2<T> sheet) {
                _Sheet = sheet;
                _SourceParentEnumerator = sheet._Source.GetEnumerator();
            }
            #endregion

            #region IEnumerator<T> Members

            public T Current {
                get {
                    var srcRow = (Ex.Relational.IRelationalRow)_SourceSubEnumerator.Current;
                    var key = Tuple.Create(_CurrentParent.Key, srcRow.Key);

                    if (_Sheet._SubRows.TryGetValue(key, out var row)) return row;

                    row = _Sheet.CreateSubRow(srcRow);
                    _Sheet._SubRows.Add(key, row);
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
                if (_SourceSubEnumerator != null && _SourceSubEnumerator.MoveNext())
                    return true;

                if (!_SourceParentEnumerator.MoveNext())
                    return false;

                _CurrentParent = (Ex.Variant2.DataRow)_SourceParentEnumerator.Current;
                _SourceSubEnumerator = _CurrentParent.SubRows.GetEnumerator();
                return _SourceSubEnumerator.MoveNext(); // Should always be true.
            }

            public void Reset() {
                _SourceParentEnumerator.Reset();
                _SourceSubEnumerator = null;
                _CurrentParent = null;
            }

            #endregion
        }
    }
}
