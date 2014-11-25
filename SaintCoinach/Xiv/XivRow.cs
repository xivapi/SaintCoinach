using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    using Ex.Relational;

    public class XivRow : IXivRow {
        #region Fields
        private IRelationalRow _SourceRow;
        private IXivSheet _Sheet;
        #endregion

        #region Properties
        public IXivSheet Sheet { get { return _Sheet; } }
        #endregion

        #region Constructor
        public XivRow(IXivSheet sheet, IRelationalRow sourceRow) {
            _Sheet = sheet;
            _SourceRow = sourceRow;
        }
        #endregion

        #region IRelationalRow Members

        IRelationalSheet IRelationalRow.Sheet {
            get { return Sheet; }
        }

        public object DefaultValue {
            get { return _SourceRow.DefaultValue; }
        }

        public object this[string columnName] {
            get { return _SourceRow[columnName]; }
        }

        #endregion

        #region IRow Members

        Ex.ISheet Ex.IRow.Sheet {
            get { return Sheet; }
        }

        public int Key {
            get { return _SourceRow.Key; }
        }

        public object this[int columnIndex] {
            get { return _SourceRow[columnIndex]; }
        }

        #endregion

        public override string ToString() {
            return _SourceRow.ToString();
        }
    }
}
