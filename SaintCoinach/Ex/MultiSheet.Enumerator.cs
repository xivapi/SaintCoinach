using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    partial class MultiSheet<TMulti, TData> {
        private class Enumerator : IEnumerator<TMulti> {
            #region Fields
            private MultiSheet<TMulti, TData> _Sheet;
            private IEnumerator<int> _KeyEnumerator;
            #endregion

            #region Constructor
            public Enumerator(MultiSheet<TMulti, TData> sheet) {
                _Sheet = sheet;
                _KeyEnumerator = sheet.ActiveSheet.Keys.GetEnumerator();
            }
            #endregion

            #region IEnumerator<TMulti> Members

            public TMulti Current {
                get { return _Sheet[_KeyEnumerator.Current]; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {
                _KeyEnumerator.Dispose();
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current {
                get { return Current; }
            }

            public bool MoveNext() {
                return _KeyEnumerator.MoveNext();
            }

            public void Reset() {
                _KeyEnumerator.Reset();
            }

            #endregion
        }
    }
}
