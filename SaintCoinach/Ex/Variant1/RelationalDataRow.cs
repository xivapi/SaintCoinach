using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Variant1 {
    using Relational;

    public class RelationalDataRow : DataRow, IRelationalDataRow {
        #region Fields
        private Dictionary<string, WeakReference<object>> _ValueReferences = new Dictionary<string, WeakReference<object>>();
        #endregion

        public new IRelationalDataSheet Sheet { get { return (IRelationalDataSheet)base.Sheet; } }

        public override string ToString() {
            var defCol = Sheet.Header.DefaultColumn;
            return defCol == null
                       ? string.Format("{0}#{1}", Sheet.Header.Name, Key)
                       : string.Format("{0}", this[defCol.Index]);
        }

        #region Constructors

        public RelationalDataRow(IDataSheet sheet, int key, int offset) : base(sheet, key, offset) { }

        #endregion

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
                object val;
                if (_ValueReferences.TryGetValue(columnName, out WeakReference<object> valRef)) {
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
