using SaintCoinach.Ex;
using SaintCoinach.Ex.Relational;
using SaintCoinach.Ex.Variant2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class XivSubRow : XivRow, IXivSubRow {
        private readonly SubRow _SourceSubRow;

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="XivSubRow" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public XivSubRow(IXivSheet sheet, IRelationalRow sourceRow)
            : base(sheet, sourceRow) {
            _SourceSubRow = (SubRow)sourceRow;
        }

        #endregion

        public string FullKey => _SourceSubRow.FullKey;

        #region IXivSubRow Members

        public IRow ParentRow => _SourceSubRow.ParentRow;

        public int ParentKey => _SourceSubRow.ParentRow.Key;

        #endregion
    }
}
