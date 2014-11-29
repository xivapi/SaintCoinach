using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class CraftCrystalType : XivRow {
        #region Properties
        public Item Item { get { return As<Item>("Item"); } }
        #endregion

        #region Constructor
        public CraftCrystalType(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}