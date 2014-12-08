using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class RecipeBookUnlock : ItemAction {
        const int RecipeGroupKey = 0;

        private Recipe[] _Recipes = null;

        #region Properties
        public int RecipeBook { get { return GetData(RecipeGroupKey); } }
        public IEnumerable<Recipe> Recipes { get { return _Recipes ?? (_Recipes = Sheet.Collection.GetSheet<Recipe>().Where(_ => _.UnlockKey == RecipeBook).ToArray()); } }
        #endregion

        #region Constructor
        public RecipeBookUnlock(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}