using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SaintCoinach.Ex {
    public abstract class DataRowBase : IDataRow {
        #region Fields

        private ConcurrentDictionary<int, WeakReference<object>> _ValueReferences;

        #endregion

        #region Constructors

        protected DataRowBase(IDataSheet sheet, int key, int offset) {
            Sheet = sheet;
            Key = key;
            Offset = offset;
            _ValueReferences = new ConcurrentDictionary<int, WeakReference<object>>();
        }

        #endregion

        public IDataSheet Sheet { get; private set; }
        ISheet IRow.Sheet { get { return Sheet; } }
        public int Key { get; private set; }
        public int Offset { get; private set; }

        #region IRow Members

        public virtual object this[int columnIndex] {
            get {
                if (_ValueReferences.ContainsKey(columnIndex) && _ValueReferences[columnIndex].TryGetTarget(out var value))
                    return value;
  
                var column = Sheet.Header.GetColumn(columnIndex);
                value = column.Read(Sheet.GetBuffer(), this);

                _ValueReferences.AddOrUpdate(columnIndex,
                    k => new WeakReference<object>(value),
                    (k, r) => {
                        r.SetTarget(value);
                        return r;
                    });
  
                return value;
            }
        }

        public virtual object GetRaw(int columnIndex) {
            var column = Sheet.Header.GetColumn(columnIndex);
            return column.ReadRaw(Sheet.GetBuffer(), this);
        }

        public IEnumerable<object> ColumnValues() {
            var buffer = Sheet.GetBuffer();
            foreach (var column in Sheet.Header.Columns)
                yield return column.Read(buffer, this);
        }

        #endregion
    }
}
