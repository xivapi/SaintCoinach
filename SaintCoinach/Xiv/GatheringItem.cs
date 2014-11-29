using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class GatheringItem : XivRow {
        #region Properties
        public Item Item { get { return As<Item>("Item"); } }
        public int ItemLevel { get { return AsInt32("ItemLevel"); } }
        public bool IsRare { get { return AsBoolean("IsRare"); } }
        #endregion

        #region Constructor
        public GatheringItem(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return string.Format("{0}", Item);
        }
    }
}