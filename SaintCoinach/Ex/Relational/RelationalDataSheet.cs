using System.Collections.Generic;
using System.Linq;

using SaintCoinach.IO;

namespace SaintCoinach.Ex.Relational {
    public class RelationalDataSheet<T> : DataSheet<T>, IRelationalDataSheet<T> where T : IRelationalDataRow {
        #region Constructors

        public RelationalDataSheet(RelationalExCollection collection, RelationalHeader header, Language language)
            : base(collection, header, language) { }

        #endregion

        public new RelationalExCollection Collection { get { return (RelationalExCollection)base.Collection; } }
        public new RelationalHeader Header { get { return (RelationalHeader)base.Header; } }

        #region Factory

        protected override ISheet<T> CreatePartialSheet(Range range, File file) {
            return new RelationalPartialDataSheet<T>(this, range, file);
        }

        #endregion

        #region IRelationalSheet Members

        IRelationalRow IRelationalSheet.this[int row] { get { return this[row]; } }

        public object this[int row, string columnName] { get { return this[row][columnName]; } }

        #endregion
    }
}
