using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public class RelationalPartialDataSheet<T> : PartialDataSheet<T>, IRelationalDataSheet<T> where T : IRelationalDataRow {
        #region Properties
        public new IRelationalDataSheet<T> SourceSheet { get { return (IRelationalDataSheet<T>)base.SourceSheet; } }
        public new RelationalHeader Header { get { return (RelationalHeader)base.Header; } }
        public new RelationalExCollection Collection { get { return (RelationalExCollection)base.Collection; } }
        #endregion

        #region Constructor
        public RelationalPartialDataSheet(IRelationalDataSheet<T> sourceSheet, Range range, IO.File file) : base(sourceSheet, range, file) { }
        #endregion

        #region IRelationalSheet Members
        IEnumerable<IRelationalRow> IRelationalSheet.GetAllRows() {
            return GetAllRows().Cast<IRelationalRow>();
        }

        IRelationalRow IRelationalSheet.this[int row] {
            get { return this[row]; }
        }

        public object this[int row, string columnName] {
            get { return this[row][columnName]; }
        }
        #endregion
    }
}
