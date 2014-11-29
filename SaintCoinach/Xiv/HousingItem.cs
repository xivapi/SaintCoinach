using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public abstract class HousingItem : XivRow {
        #region Properties
        public HousingItemCategory HousingItemCategory { get { return As<HousingItemCategory>(); } }
        public Item Item { get { return As<Item>("Item"); } }
        #endregion

        #region Constructor
        public HousingItem(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return string.Format("{0}", Item);
        }
    }
}