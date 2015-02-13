namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing an ingredient in a <see cref="Recipe" />.
    /// </summary>
    public class RecipeIngredient {
        #region Properties

        /// <summary>
        ///     Gets the <see cref="RecipeIngredientType" /> of the current ingredient.
        /// </summary>
        /// <value>The <see cref="RecipeIngredientType" /> of the current ingredient.</value>
        public RecipeIngredientType Type { get; private set; }

        /// <summary>
        ///     Gets the <see cref="Item" /> of the current ingredient.
        /// </summary>
        /// <value>The <see cref="Item" /> of the current ingredient.</value>
        public Item Item { get; private set; }

        /// <summary>
        ///     Gets the item count for the current ingredient.
        /// </summary>
        /// <value>The item count for the current ingredient.</value>
        public int Count { get; private set; }

        /// <summary>
        ///     Gets the quality gained per high-quality item used for the current ingredient.
        /// </summary>
        /// <value>The quality gained per high-quality item used for the current ingredient.</value>
        public int QualityPerItem { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecipeIngredient" /> class.
        /// </summary>
        /// <param name="type"><see cref="RecipeIngredientType" /> for the ingredient.</param>
        /// <param name="item"><see cref="Item" /> for the ingredient.</param>
        /// <param name="count">Item count for the ingredient.</param>
        public RecipeIngredient(RecipeIngredientType type, Item item, int count) {
            Type = type;
            Item = item;
            Count = count;
        }

        #endregion
    }
}
