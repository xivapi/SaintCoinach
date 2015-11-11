using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a crafting recipe.
    /// </summary>
    public class Recipe : XivRow, IItemSource {
        #region Fields

        /// <summary>
        ///     Ingredients of the current recipe.
        /// </summary>
        private RecipeIngredient[] _Ingredients;

        /// <summary>
        ///     <see cref="RecipeLevel" /> of the current recipe.
        /// </summary>
        private RecipeLevel _RecipeLevel;

        /// <summary>
        ///     A value indicating whether the <see cref="UnlockItem"/> has been fetched.
        /// </summary>
        private bool _UnlockItemFetched;

        /// <summary>
        ///     <see cref="Item"/> required to unlock the current recipe.
        /// </summary>
        private Item _UnlockItem;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the <see cref="CraftType" /> of the current recipe.
        /// </summary>
        /// <value>The <see cref="CraftType" /> of the current recipe.</value>
        public CraftType CraftType { get { return As<CraftType>(); } }

        /// <summary>
        ///     Gets the <see cref="ClassJob" /> of the current recipe.
        /// </summary>
        /// <value>The <see cref="ClassJob" /> of the current recipe.</value>
        public ClassJob ClassJob { get { return CraftType.ClassJob; } }

        /// <summary>
        ///     Gets the <see cref="RecipeElement" /> of the current recipe.
        /// </summary>
        /// <value>The <see cref="RecipeElement" /> of the current recipe.</value>
        public RecipeElement RecipeElement { get { return As<RecipeElement>(); } }

        /// <summary>
        ///     Gets the <see cref="RecipeLevel" /> of the current recipe.
        /// </summary>
        /// <value>The <see cref="RecipeLevel" /> of the current recipe.</value>
        public RecipeLevel RecipeLevel { get { return _RecipeLevel ?? (_RecipeLevel = new RecipeLevel(this)); } }

        /// <summary>
        ///     Gets the <see cref="Item" /> created by the current recipe.
        /// </summary>
        /// <value>The <see cref="Item" /> created by the current recipe.</value>
        public Item ResultItem { get { return As<Item>("Item{Result}"); } }

        /// <summary>
        ///     Gets the number of items created by the current recipe.
        /// </summary>
        /// <value>The number of items created by the current recipe.</value>
        public int ResultCount { get { return AsInt32("Amount{Result}"); } }

        /// <summary>
        ///     Gets the <see cref="RecipeIngredient" />s of the current recipe.
        /// </summary>
        /// <value>The <see cref="RecipeIngredient" />s of the current recipe.</value>
        public IEnumerable<RecipeIngredient> Ingredients {
            get { return _Ingredients ?? (_Ingredients = BuildIngredients()); }
        }

        /// <summary>
        ///     Gets a value indicating whether the current recipe uses the secondary tool.
        /// </summary>
        /// <value>A value indicating whether the current recipe uses the secondary tool.</value>
        public bool IsSecondary { get { return AsBoolean("IsSecondary"); } }

        /// <summary>
        ///     Gets a value indicating whether the current recipe can be used for quick synthesis.
        /// </summary>
        /// <value>A value indicating whether the current recipe can be used for quick synthesis..</value>
        public bool CanQuickSynth { get { return AsBoolean("CanQuickSynth"); } }

        /// <summary>
        ///     Gets a value indicating whether the current recipe can produce high-quality items.
        /// </summary>
        /// <value>A value indicating whether the current recipe can produce high-quality items.</value>
        public bool CanHq { get { return AsBoolean("CanHq"); } }

        /// <summary>
        ///     Gets the required craftsmanship value to attempt the current recipe.
        /// </summary>
        /// <value>The required craftsmanship value to attempt the current recipe.</value>
        public int RequiredCraftsmanship { get { return AsInt32("RequiredCraftsmanship"); } }

        /// <summary>
        ///     Gets the required control value to attempt the current recipe.
        /// </summary>
        /// <value>The required control value to attempt the current recipe.</value>
        public int RequiredControl { get { return AsInt32("RequiredControl"); } }

        /// <summary>
        ///     Gets the required craftsmanship value to quick synth the current recipe.
        /// </summary>
        /// <value>The required craftsmanship value to quick synth the current recipe.</value>
        public int QuickSynthCraftsmanship { get { return AsInt32("QuickSynthCraftsmanship"); } }

        /// <summary>
        ///     Gets the required control value to quick synth the current recipe.
        /// </summary>
        /// <value>The required control value to quick synth the current recipe.</value>
        public int QuickSynthControl { get { return AsInt32("QuickSynthControl"); } }

        /// <summary>
        ///     Gets the required <see cref="Status" /> to attempt the current recipe.
        /// </summary>
        /// <value>The required <see cref="Status" /> to attempt the current recipe.</value>
        public Status RequiredStatus { get { return As<Status>("Status{Required}"); } }

        /// <summary>
        ///     Gets the required equipped <see cref="Item" /> to attempt the current recipe.
        /// </summary>
        /// <value>The required equipped <see cref="Item" /> to attempt the current recipe.</value>
        public Item RequiredItem { get { return As<Item>("Item{Required}"); } }

        /// <summary>
        ///     Gets the <see cref="RecipeLevelTable" /> used by the current recipe.
        /// </summary>
        /// <value>The <see cref="RecipeLevelTable" /> used by the current recipe.</value>
        public RecipeLevelTable RecipeLevelTable { get { return As<RecipeLevelTable>(); } }

        /// <summary>
        ///     Gets the factor, in percent, to apply to the difficulty of the the current recipe's <see cref="RecipeLevelTable" />
        ///     .
        /// </summary>
        /// <value>The factor, in percent, to apply to the difficulty of the the current recipe's <see cref="RecipeLevelTable" />.</value>
        public int DifficultyFactor { get { return AsInt32("DifficultyFactor"); } }

        /// <summary>
        ///     Gets the factor, in percent, to apply to the quality of the the current recipe's <see cref="RecipeLevelTable" />.
        /// </summary>
        /// <value>The factor, in percent, to apply to the quality of the the current recipe's <see cref="RecipeLevelTable" />.</value>
        public int QualityFactor { get { return AsInt32("QualityFactor"); } }

        /// <summary>
        ///     Gets the factor, in percent, to apply to the durability of the the current recipe's <see cref="RecipeLevelTable" />
        ///     .
        /// </summary>
        /// <value>The factor, in percent, to apply to the durability of the the current recipe's <see cref="RecipeLevelTable" />.</value>
        public int DurabilityFactor { get { return AsInt32("DurabilityFactor"); } }

        /// <summary>
        ///     Gets the key required to unlock the current recipe.
        /// </summary>
        /// <remarks>
        ///     This is the same key as used in <see cref="ItemActions.RecipeBookUnlock" />.
        /// </remarks>
        /// <value>The key required to unlock the current recipe.</value>
        public int UnlockKey { get { return AsInt32("UnlockKey"); } }

        /// <summary>
        ///     Gets the <see cref="Item"/> required to unlock the current recipe.
        /// </summary>
        /// <value>The <see cref="Item"/> required to unlock the current recipe.</value>
        public Item UnlockItem {
            get {
                if (!_UnlockItemFetched) {
                    if (UnlockKey != 0) {
                        var itemAction = Sheet.Collection.GetSheet<ItemAction>().OfType<ItemActions.RecipeBookUnlock>().FirstOrDefault(i => i.RecipeBook == this.UnlockKey);
                        _UnlockItem = Sheet.Collection.GetSheet<Item>().FirstOrDefault(i => i.ItemAction == itemAction);
                    }
                    _UnlockItemFetched = true;
                }
                return _UnlockItem;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the current recipe requires a specialization soul.
        /// </summary>
        /// <value>A value indicating whether the current recipe requires a specialization soul.</value>
        public bool IsSpecializationRequired {  get { return AsBoolean("IsSpecializationRequired"); } }
        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Recipe" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public Recipe(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build

        /// <summary>
        ///     Build an array of <see cref="RecipeIngredient" />s used in the current recipe.
        /// </summary>
        /// <returns>An array of <see cref="RecipeIngredient" />s used in the current recipe.</returns>
        private RecipeIngredient[] BuildIngredients() {
            const int MaterialCount = 8;
            const int CrystalCount = 2;

            var ingredients = new List<RecipeIngredient>();

            for (var i = 0; i < MaterialCount; ++i) {
                var item = As<Item>("Item{Ingredient}", i);
                if (item.Key == 0)
                    continue;

                var count = AsInt32("Amount{Ingredient}", i);
                if (count == 0)
                    continue;

                ingredients.Add(new RecipeIngredient(RecipeIngredientType.Material, item, count));
            }
            for (var i = 0; i < CrystalCount; ++i) {
                var craftCrystal = As<CraftCrystalType>("CraftCrystalType", i);
                if (craftCrystal == null)
                    continue;

                var count = AsInt32("Amount{Crystal}", i);
                if (count == 0)
                    continue;

                ingredients.Add(new RecipeIngredient(RecipeIngredientType.Crystal, craftCrystal.Item, count));
            }
            var itemLevelSum =
                ingredients.Where(_ => _.Type == RecipeIngredientType.Material)
                           .Sum(_ => _.Count * _.Item.ItemLevel.Key);
            var qualityFromMats = RecipeLevel.Quality / 2;
            foreach (var mat in ingredients.Where(_ => _.Type == RecipeIngredientType.Material))
                mat.QualityPerItem = mat.Item.ItemLevel.Key * qualityFromMats / itemLevelSum;

            return ingredients.ToArray();
        }

        #endregion

        #region IItemSource Members

        IEnumerable<Item> IItemSource.Items {
            get { yield return this.ResultItem; }
        }

        #endregion
    }
}
