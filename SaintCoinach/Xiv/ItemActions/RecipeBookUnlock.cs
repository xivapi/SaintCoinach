using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.ItemActions {
    public class RecipeBookUnlock : ItemAction {
        #region Static

        private const int RecipeGroupKey = 0;

        #endregion

        #region Fields

        private Recipe[] _Recipes;

        #endregion

        #region Properties

        public int RecipeBook { get { return GetData(RecipeGroupKey); } }

        public IEnumerable<Recipe> Recipes {
            get {
                return _Recipes
                       ?? (_Recipes =
                           Sheet.Collection.GetSheet<Recipe>().Where(_ => _.UnlockKey == RecipeBook).ToArray());
            }
        }

        #endregion

        #region Constructors

        #region Constructor

        public RecipeBookUnlock(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
