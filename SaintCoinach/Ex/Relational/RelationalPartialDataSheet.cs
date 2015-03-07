using System.Collections.Generic;
using System.Linq;

using SaintCoinach.IO;

namespace SaintCoinach.Ex.Relational {
    public class RelationalPartialDataSheet<T> : PartialDataSheet<T>, IRelationalDataSheet<T>
        where T : IRelationalDataRow {
        #region Properties

        public new IRelationalDataSheet<T> SourceSheet { get { return (IRelationalDataSheet<T>)base.SourceSheet; } }

        #endregion

        #region Constructors

        public RelationalPartialDataSheet(IRelationalDataSheet<T> sourceSheet, Range range, File file)
            : base(sourceSheet, range, file) { }

        #endregion

        public new RelationalHeader Header { get { return (RelationalHeader)base.Header; } }
        public new RelationalExCollection Collection { get { return (RelationalExCollection)base.Collection; } }

        #region IRelationalSheet Members

        IRelationalRow IRelationalSheet.this[int row] { get { return this[row]; } }

        public object this[int row, string columnName] { get { return this[row][columnName]; } }

        #endregion
    }
}
