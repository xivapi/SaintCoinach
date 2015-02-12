using System;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ItemLevel : XivRow {
        #region Constructors

        #region Constructor

        public ItemLevel(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Helpers

        public int GetMaximum(BaseParam baseParam) {
            const int Offset = -1;
            return baseParam.Key == 0 ? 0 : Convert.ToInt32(this[Offset + baseParam.Key]);
        }

        #endregion

        public override string ToString() {
            return Key.ToString();
        }
    }
}
