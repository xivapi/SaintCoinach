using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public class RelationalDataSheet<T> : DataSheet<T>, IRelationalDataSheet<T> where T : IRelationalDataRow {
        #region Properties
        public new RelationalExCollection Collection { get { return (RelationalExCollection)base.Collection; } }
        public new RelationalHeader Header { get { return (RelationalHeader)base.Header; } }
        #endregion

        #region Constructor
        public RelationalDataSheet(RelationalExCollection collection, RelationalHeader header, Language language) : base(collection, header, language) { }
        #endregion

        #region Factory
        protected override ISheet<T> CreatePartialSheet(Range range, IO.File file) {
            return new RelationalPartialDataSheet<T>(this, range, file);
        }
        #endregion

        #region IRelationalSheet Members
        IEnumerable<IRelationalRow> IRelationalSheet.GetAllRows() {
            return GetAllRows().Cast<IRelationalRow>();
        }

        IRelationalRow IRelationalSheet.this[int row] {
            get { return this[row]; }
        }

        public object this[int row, string columnName] {
            get { return this[row, columnName]; }
        }
        #endregion
    }
}
