namespace SaintCoinach.Xiv {
    public class RecipeLevel {
        #region Properties

        public RecipeLevelTable RecipeLevelTable { get; private set; }
        public int Difficulty { get; private set; }
        public int Quality { get; private set; }
        public int Durability { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public RecipeLevel(Recipe recipe) {
            RecipeLevelTable = recipe.RecipeLevelTable;
            Difficulty = (RecipeLevelTable.Difficulty * recipe.DifficultyFactor) / 100;
            Quality = (RecipeLevelTable.Quality * recipe.QualityFactor) / 100;
            Durability = (RecipeLevelTable.Durability * recipe.DurabilityFactor) / 100;
        }

        #endregion

        #endregion
    }
}
