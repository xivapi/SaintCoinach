using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ItemLevel : XivRow {
        #region Constructor
        public ItemLevel(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Helpers
        public int GetMaximum(BaseParam baseParam) {
            const int Offset = -1;
            if(baseParam.Key == 0)
                return 0;
            return Convert.ToInt32(this[Offset + baseParam.Key]);
        }
        #endregion
    }
}