using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class CompanyCraftSupplyItem : XivRow {

        #region Properties

        public Item Item { get { return As<Item>(); } }

        #endregion
        
        #region Constructors

        public CompanyCraftSupplyItem(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
