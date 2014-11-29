using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ItemAction : XivRow {
        public const int DataCount = 8;

        #region Properties
        public int Type { get { return AsInt32("Type"); } }
        #endregion

        #region Constructor
        public ItemAction(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Helpers
        public int GetData(int index) {
            return AsInt32("Data", index);
        }
        public int GetHqData(int index) {
            return AsInt32("Data{HQ}", index);
        }
        #endregion
    }
}