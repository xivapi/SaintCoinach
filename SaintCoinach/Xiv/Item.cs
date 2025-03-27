using System;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing items that are in the default inventory.
    /// </summary>
    public class Item : ItemBase {
        #region Fields

        /// <summary>
        ///     Sources from which to obtain the current item.
        /// </summary>
        private IItemSource[] _Sources;

        /// <summary>
        ///     Listings offering the current item as a reward.
        /// </summary>
        private IShopListing[] _AsShopItems;

        /// <summary>
        ///     Listings requiring the current item as payment.
        /// </summary>
        private IShopListingItem[] _AsShopPayment;

        /// <summary>
        ///     Recipes using the current item as material.
        /// </summary>
        private Recipe[] _RecipesAsMaterial;

        /// <summary>
        ///     Recipes creating the current item.
        /// </summary>
        private Recipe[] _RecipesAsResult;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the <see cref="ItemLevel" /> of the current item.
        /// </summary>
        /// <value>The <see cref="ItemLevel" /> of the current item.</value>
        public ItemLevel ItemLevel { get { return As<ItemLevel>("Level{Item}"); } }

        /// <summary>
        ///     Gets a value indicating whether the current item is unique.
        /// </summary>
        /// <value>A value indicating whether the current item is unique.</value>
        public bool IsUnique { get { return AsBoolean("IsUnique"); } }

        /// <summary>
        ///     Gets a value indicating whether the current item is untradable.
        /// </summary>
        /// <value>A value indicating whether the current item is untradable.</value>
        public bool IsUntradable { get { return AsBoolean("IsUntradable"); } }

        /// <summary>
        ///     Gets a value indicating whether the current item is indisposable.
        /// </summary>
        /// <remarks>If <c>true</c> the current item can not be disposed of or sold to NPCs.</remarks>
        /// <value>A value indicating whether the current item is indisposable.</value>
        public bool IsIndisposable { get { return AsBoolean("IsIndisposable"); } }

        /// <summary>
        ///     Gets a value indicating whether the current item is dyeable.
        /// </summary>
        /// <value>A value indicating whether the current item is dyeable.</value>
        public bool IsDyeable { get { return AsInt16("DyeCount") > 0; } }

        /// <summary>
        /// Gets a value indicating whether the current item is collectable.
        /// </summary>
        /// <value>A value indicaating whether the current item is collectable.</value>
        public bool IsCollectable { get { return AsBoolean("IsCollectable"); } }

        /// <summary>
        ///     Gets a value indicating whether the current item can have materia melded with advanced melding.
        /// </summary>
        /// <value>A value indicating whether the current item can have materia melded with advanced melding.</value>
        public bool IsAdvancedMeldingPermitted { get { return AsBoolean("IsAdvancedMeldingPermitted"); } }

        /// <summary>
        ///     Gets the additional data row associated with the current item.
        /// </summary>
        /// <value>The additional data row associated with the current item.</value>
        public IXivRow AdditionalData => As<IXivRow>("AdditionalData");

        /// <summary>
        ///     Gets the <see cref="EquipSlotCategory" /> of the current item.
        /// </summary>
        /// <value>The <see cref="EquipSlotCategory" /> of the current item.</value>
        public EquipSlotCategory EquipSlotCategory { get { return As<EquipSlotCategory>(); } }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        ///     Gets the <see cref="ItemUICategory" /> of the current item.
        /// </summary>
        /// <value>The <see cref="ItemUICategory" /> of the current item.</value>
        public ItemUICategory ItemUICategory { get { return As<ItemUICategory>(); } }

        /// <summary>
        ///     Gets the <see cref="ItemSearchCategory" /> of the current item.
        /// </summary>
        /// <value>The <see cref="ItemSearchCategory" /> of the current item.</value>
        public ItemSearchCategory ItemSearchCategory { get { return As<ItemSearchCategory>(); } }

        /// <summary>
        ///     Gets the <see cref="ItemAction" /> invoked when the current item is used.
        /// </summary>
        /// <value>The <see cref="ItemAction" /> invoked when the current item is used.</value>
        public ItemAction ItemAction { get { return As<ItemAction>(); } }

        /// <summary>
        ///     Gets the price NPCs offer when selling the current item.
        /// </summary>
        /// <value>The price NPCs offer when selling the current item.</value>
        public int Bid { get { return AsInt32("Price{Low}"); } }

        /// <summary>
        ///     Gets the price in Gil of the current item in shops.
        /// </summary>
        /// <value>The price in Gil of the current item in shops.</value>
        public int Ask { get { return AsInt32("Price{Mid}"); } }

        /// <summary>
        ///     Gets the recipes using the current item as material.
        /// </summary>
        /// <remarks>
        ///     This property does not include recipes using the current item as catalyst (crystals).
        /// </remarks>
        /// <value>The recipes using the current item as material.</value>
        public IEnumerable<Recipe> RecipesAsMaterial {
            get { return _RecipesAsMaterial ?? (_RecipesAsMaterial = BuildRecipesAsMaterial()); }
        }

        /// <summary>
        ///     Gets the recipes creating the current item.
        /// </summary>
        /// <value>The recipes creating the current item.</value>
        public IEnumerable<Recipe> RecipesAsResult {
            get { return _RecipesAsResult ?? (_RecipesAsResult = BuildRecipesAsResult()); }
        }

        /// <summary>
        ///     Gets the shop listings offering the current item as reward.
        /// </summary>
        /// <value>The shop listings offering the current item as reward.</value>
        public IEnumerable<IShopListing> AsShopItems {
            get { return _AsShopItems ?? (_AsShopItems = BuildAsShopItems()); }
        }

        /// <summary>
        ///     Gets the shop listings requiring the current item as payment.
        /// </summary>
        /// <value>The shop listings requiring the current item as payment.</value>
        public IEnumerable<IShopListingItem> AsShopPayment {
            get { return _AsShopPayment ?? (_AsShopPayment = BuildAsShopPayment()); }
        }

        /// <summary>
        ///     Gets the sources from which to obtain the current item.
        /// </summary>
        /// <value>The sources from which to obtain the current item.</value>
        public IEnumerable<IItemSource> Sources {
            get { return _Sources ?? (_Sources = BuildSources()); }
        }

        /// <summary>
        ///    Gets the rarity of the current item.
        /// </summary>
        /// <value>The rarity of the current item.</value>
        /// <remarks>
        /// 1: Common (White)
        /// 2: Uncommon (Green)
        /// 3: Rare (Blue)
        /// 4: Relic (Purple)
        /// 7: Aetherial (Pink)
        /// </remarks>
        public int Rarity {
            get { return AsInt32("Rarity"); }
        }

        /// <summary>
        /// Gets a flag indicating whether HQ versions of the current item exist.
        /// </summary>
        /// <value>A flag indicating whether HQ versions of the current item exist.</value>
        public bool CanBeHq {
            get { return AsBoolean("CanBeHq"); }
        }

        public GrandCompany GrandCompany {
            get { return As<GrandCompany>(); }
        }

        public Salvage Salvage {
            get { return As<Salvage>(); }
        }

        public bool IsAetherialReducible {
            get { return AsInt32("AetherialReduce") > 0; }
        }

        public bool IsConvertable {
            get { return As<byte>("MaterializeType") > 0; }
        }

        public bool IsGlamourous => AsBoolean("IsGlamourous");

        public Quad ModelMain { get { return AsQuad("Model{Main}"); } }

        public Quad ModelSub { get { return AsQuad("Model{Sub}"); } }

        /// <summary>
        ///     Gets the <see cref="ClassJob" /> required to repair or desynth the current item.
        /// </summary>
        /// <value>The <see cref="ClassJob" /> required to repair or desynth the current item.</value>
        public ClassJob RepairClassJob => As<ClassJob>("ClassJob{Repair}");
        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Item" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public Item(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build

        /// <summary>
        ///     Build an array of recipes using the current item as material.
        /// </summary>
        /// <returns>An array of recipes using the current item as material.</returns>
        private Recipe[] BuildRecipesAsMaterial() {
            var rSheet = Sheet.Collection.GetSheet<Recipe>();

            return
                rSheet.Where(
                             recipe =>
                             recipe.Ingredients.Any(ing => ing.Type == RecipeIngredientType.Material && ing.Item == this))
                      .ToArray();
        }

        /// <summary>
        ///     Build an array of recipes creating the current item.
        /// </summary>
        /// <returns>An array of recipes creating the current item.</returns>
        private Recipe[] BuildRecipesAsResult() {
            var rSheet = Sheet.Collection.GetSheet<Recipe>();

            return rSheet.Where(recipe => recipe.ResultItem == this).ToArray();
        }

        /// <summary>
        ///     Build an array of shop listings offering the current item as reward.
        /// </summary>
        /// <returns>An array of shop listings offering the current item as reward.</returns>
        private IShopListing[] BuildAsShopItems() {
            var shops = Sheet.Collection.Shops;

            var shopItems = new List<IShopListing>();
            foreach (var shop in shops)
                shopItems.AddRange(shop.ShopListings.Where(l => l.Rewards.Any(li => li.Item == this)));
            return shopItems.Distinct().ToArray();
        }

        /// <summary>
        ///     Build an array of shop listings requiring the current item as payment.
        /// </summary>
        /// <returns>An array of shop listings requiring the current item as payment.</returns>
        private IShopListingItem[] BuildAsShopPayment() {
            if (Key == 1)
                return new IShopListingItem[0]; // XXX: DO NOT BUILD THIS FOR GIL, THAT WOULD BE BAD.

            var shops = Sheet.Collection.Shops;

            var checkedItems = new List<IShopListing>();
            var shopItemCosts = new List<IShopListingItem>();
            foreach (var item in shops.SelectMany(shop => shop.ShopListings.Except(checkedItems).ToArray())) {
                shopItemCosts.AddRange(item.Costs.Where(_ => _.Item == this));
                checkedItems.Add(item);
            }
            return shopItemCosts.Distinct().ToArray();
        }

        /// <summary>
        ///     Build an array of sources from which to obtain the current item.
        /// </summary>
        /// <returns>An array of sources from which to obtain the current item.</returns>
        private IItemSource[] BuildSources() {
            var sources = new List<IItemSource>();

            Libra.Item libraRow = null;
            if (Sheet.Collection.IsLibraAvailable)
                libraRow = Sheet.Collection.Libra.Items.FirstOrDefault(i => i.Key == this.Key);

            var recipes = Sheet.Collection.GetSheet<Recipe>();
            var quests = Sheet.Collection.GetSheet<Quest>();
            var achievements = Sheet.Collection.GetSheet<Achievement>();
            var shops = Sheet.Collection.Shops;
            var leves = Sheet.Collection.GetSheet<Leve>();
            var fishingSpots = Sheet.Collection.GetSheet<FishingSpot>();
            var retainerTasks = Sheet.Collection.GetSheet<RetainerTask>();
            var companyCraft = Sheet.Collection.GetSheet<CompanyCraftSequence>();

            if (libraRow != null) {
                var bnpcColl = Sheet.Collection.BNpcs;
                var instanceContents = Sheet.Collection.GetSheet<InstanceContent>();

                foreach (var bnpc in libraRow.BNpcs)
                    sources.Add(bnpcColl[bnpc]);
                foreach (var ic in libraRow.InstanceContents)
                    sources.Add(instanceContents[ic]);
            }

            /*sources.AddRange(bnpcColl.Where(i => i.Items.Contains(this)));
            sources.AddRange(instanceContents.Cast<IItemSource>().Where(i => i.Items.Contains(this)));*/

            // Not using Libra for these because it has a higher likelyhood of being incomplete.
            sources.AddRange(recipes.Where(i => i.ResultItem == this));
            sources.AddRange(quests.Cast<IItemSource>().Where(i => i.Items.Contains(this)));
            sources.AddRange(achievements.Where(i => i.Item == this));
            sources.AddRange(shops.Where(i => i.Items.Contains(this)));

            // Not using Libra for this because it doesn't even have information about it
            sources.AddRange(leves.Where(i => i.Items.Contains(this)));
            sources.AddRange(fishingSpots.Where(i => i.Items.Contains(this)));
            sources.AddRange(retainerTasks.Where(i => i.Items.Contains(this)));
            sources.AddRange(companyCraft.Where(i => i.ResultItem == this));

            return sources.ToArray();
        }

        #endregion
    }
}
