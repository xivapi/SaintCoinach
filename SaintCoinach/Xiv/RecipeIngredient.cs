namespace SaintCoinach.Xiv {
    public class RecipeIngredient {
        #region Properties

        public RecipeIngredientType Type { get; private set; }
        public Item Item { get; private set; }
        public int Count { get; private set; }
        public int QualityPerItem { get; internal set; }

        #endregion

        #region Constructors

        #region Constructor

        public RecipeIngredient(RecipeIngredientType type, Item item, int count) {
            Type = type;
            Item = item;
            Count = count;
        }

        #endregion

        #endregion
    }
}
