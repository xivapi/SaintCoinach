using System;
using System.Collections.Generic;

namespace SaintCoinach.Ex.Relational {
    public class RelationalDataRow : DataRow, IRelationalDataRow {
        #region Fields
        private Dictionary<string, WeakReference<object>> _ValueReferences = new Dictionary<string, WeakReference<object>>();
        #endregion

        #region Constructors

        public RelationalDataRow(IRelationalDataSheet sheet, int key, int offset) : base(sheet, key, offset) { }

        #endregion

        public new IRelationalDataSheet Sheet { get { return (IRelationalDataSheet)base.Sheet; } }

        public override string ToString() {
            var defCol = Sheet.Header.DefaultColumn;
            return defCol == null
                       ? string.Format("{0}#{1}", Sheet.Header.Name, Key)
                       : string.Format("{0}", this[defCol.Index]);
        }

        #region IRelationalRow Members

        IRelationalSheet IRelationalRow.Sheet { get { return Sheet; } }

        public object DefaultValue {
            get {
                var defCol = Sheet.Header.DefaultColumn;
                return defCol == null ? null : this[defCol.Index];
            }
        }

        public object this[string columnName] {
            get {
                WeakReference<object> valRef;
                object val;
                if (_ValueReferences.TryGetValue(columnName, out valRef)) {
                    if (valRef.TryGetTarget(out val))
                        return val;
                    _ValueReferences.Remove(columnName);
                }

                var col = Sheet.Header.FindColumn(columnName);
                if (col == null)
                    throw new KeyNotFoundException();
                val = this[col.Index];

                _ValueReferences.Add(columnName, new WeakReference<object>(val));
                return val;
            }
        }

        object IRelationalRow.GetRaw(string columnName) {
            var column = Sheet.Header.FindColumn(columnName);
            if (column == null)
                throw new KeyNotFoundException();
            return column.ReadRaw(Sheet.GetBuffer(), this);
        }

        #endregion
    }
}
