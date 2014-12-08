using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class Food : Enhancement {
        #region Constructor
        public Food(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}