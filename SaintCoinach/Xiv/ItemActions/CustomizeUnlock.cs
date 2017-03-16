using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class CustomizeUnlock : ItemAction {
        #region Static

        private const int CustomizeDataKey = 0;
        private const int LogMessageKey = 1;

        #endregion

        #region Properties

        public IEnumerable<IXivRow> CustomizeRows {
            get {
                var data = (UInt16)GetData(CustomizeDataKey);
                var customize = Sheet.Collection.GetSheet("CharaMakeCustomize");
                return customize.Cast<IXivRow>().Where(r => (UInt16)r["Data"] == data).ToArray();
            }
        }

        #endregion

        #region Constructors

        #region Constructor

        public CustomizeUnlock(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
