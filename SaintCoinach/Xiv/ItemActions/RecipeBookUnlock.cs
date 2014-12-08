using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class RecipeBookUnlock : ItemAction {
        const int RecipeGroupKey = 0;

        #region Properties
        public int RecipeBook { get { return GetData(RecipeGroupKey); } }
        #endregion

        #region Constructor
        public RecipeBookUnlock(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}