using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Variant2 {
    public class SubRow : DataRowBase, IRelationalDataRow {
        public IDataRow ParentRow { get; private set; }
        public string FullKey {
            get { return ParentRow.Key + "." + Key; }
        }

        #region Constructors

        public SubRow(IDataRow parent, int key, int offset) : base(parent.Sheet, key, offset) {
            ParentRow = parent;
        }

        #endregion

        #region IRelationalRow Members

        public new IRelationalDataSheet Sheet { get { return (IRelationalDataSheet)base.Sheet; } }

        IRelationalDataSheet IRelationalDataRow.Sheet { get { return Sheet; } }

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

        object IRelationalRow.GetRaw(string columnName) {
            var column = Sheet.Header.FindColumn(columnName);
            if (column == null)
                throw new KeyNotFoundException();
            return this.GetRaw(column.Index);
        }

        #endregion
    }
}
