using System.Collections.Generic;

namespace SaintCoinach.Ex.Relational {
    public class RelationalDataRow : DataRow, IRelationalDataRow {
        #region Constructors

        #region Constructor

        public RelationalDataRow(IRelationalDataSheet sheet, int key, int offset) : base(sheet, key, offset) { }

        #endregion

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
                var col = Sheet.Header.FindColumn(columnName);
                if (col == null)
                    throw new KeyNotFoundException();
                return this[col.Index];
            }
        }

        #endregion
    }
}
