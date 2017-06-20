using System;
using System.Collections.Generic;

namespace SaintCoinach.Ex {
    public abstract class DataRowBase : IDataRow {
        #region Fields

        private Dictionary<int, WeakReference<object>> _ValueReferences;

        #endregion

        #region Constructors

        protected DataRowBase(IDataSheet sheet, int key, int offset) {
            Sheet = sheet;
            Key = key;
            Offset = offset;
            _ValueReferences = new Dictionary<int, WeakReference<object>>();
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
  
                if (_ValueReferences.ContainsKey(columnIndex))
                    _ValueReferences[columnIndex].SetTarget(value);
                else
                    _ValueReferences.Add(columnIndex, new WeakReference<object>(value));
  
                return value;
            }
        }

        public virtual object GetRaw(int columnIndex)
        {
            var column = Sheet.Header.GetColumn(columnIndex);
            return column.ReadRaw(Sheet.GetBuffer(), this);
        }

        #endregion
    }
}
