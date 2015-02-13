namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing the level and parameters for a specific <see cref="Recipe" />.
    /// </summary>
    public class RecipeLevel {
        #region Properties

        /// <summary>
        ///     Gets the <see cref="RecipeLevelTable" /> used for the current recipe.
        /// </summary>
        /// <value>The <see cref="RecipeLevelTable" /> used for the current recipe.</value>
        public RecipeLevelTable RecipeLevelTable { get; private set; }

        /// <summary>
        ///     Gets the difficulty (required progress) of the current recipe.
        /// </summary>
        /// <value>The difficulty (required progress) of the current recipe.</value>
        public int Difficulty { get; private set; }

        /// <summary>
        ///     Gets the maximum quality of the current recipe.
        /// </summary>
        /// <value>The maximum quality of the current recipe.</value>
        public int Quality { get; private set; }

        /// <summary>
        ///     Gets the maximum durability of the current recipe.
        /// </summary>
        /// <value>The maximum durability of the current recipe.</value>
        public int Durability { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecipeLevel" /> class.
        /// </summary>
        /// <param name="recipe"><see cref="Recipe" /> for which to fill the properties.</param>
        public RecipeLevel(Recipe recipe) {
            RecipeLevelTable = recipe.RecipeLevelTable;
            Difficulty = (RecipeLevelTable.Difficulty * recipe.DifficultyFactor) / 100;
            Quality = (RecipeLevelTable.Quality * recipe.QualityFactor) / 100;
            Durability = (RecipeLevelTable.Durability * recipe.DurabilityFactor) / 100;
        }

        #endregion
    }
}
