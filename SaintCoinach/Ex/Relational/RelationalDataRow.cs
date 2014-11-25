using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public class RelationalDataRow : DataRow, IRelationalDataRow {
        #region Properties
        public new IRelationalDataSheet Sheet { get { return (IRelationalDataSheet)base.Sheet; } }
        #endregion

        #region Constructor
        public RelationalDataRow(IRelationalDataSheet sheet, int key, int offset) : base(sheet, key, offset) { }
        #endregion

        #region IRelationalRow Members
        IRelationalSheet IRelationalRow.Sheet { get { return Sheet; } }
        public object DefaultValue {
            get {
                var defCol = Sheet.Header.DefaultColumn;
                if (defCol == null)
                    return null;
                return this[defCol.Index];
            }
        }

        public object this[string columnName] {
            get {
                var col = Sheet.Header.FindColumn(columnName);
                if (col == null)
                    throw new KeyNotFoundException();
                return this[col.Index];
            }
        }

        #endregion

        public override string ToString() {
            var defCol = Sheet.Header.DefaultColumn;
            if (defCol == null)
                return string.Format("{0}#{1}", Sheet.Header.Name, Key);
            return string.Format("{0}", this[defCol.Index]);
        }
    }
}
