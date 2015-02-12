using System;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class Item : ItemBase {
        #region Static

        private const double BidFactor = 0.05;

        #endregion

        #region Fields

        private IShopListing[] _AsShopItems;
        private IShopListingItem[] _AsShopPayment;
        private Recipe[] _RecipesAsMaterial;
        private Recipe[] _RecipesAsResult;

        #endregion

        #region Properties

        public ItemLevel ItemLevel { get { return As<ItemLevel>("Level{Item}"); } }
        public bool IsUnique { get { return AsBoolean("IsUnique"); } }
        public bool IsUntradable { get { return AsBoolean("IsUntradable"); } }
        public bool IsIndisposable { get { return AsBoolean("IsIndisposable"); } }
        public bool IsDyeable { get { return AsBoolean("IsDyeable"); } }
        public Stain Stain { get { return As<Stain>(); } }
        // ReSharper disable once InconsistentNaming
        public ItemUICategory ItemUICategory { get { return As<ItemUICategory>(); } }
        public ItemSearchCategory ItemSearchCategory { get { return As<ItemSearchCategory>(); } }
        public ItemAction ItemAction { get { return As<ItemAction>(); } }
        public int Bid { get { return (int)Math.Max(1, Math.Round(Ask * BidFactor)); } }
        public int Ask { get { return AsInt32("Price{Mid}"); } }

        public IEnumerable<Recipe> RecipesAsMaterial {
            get { return _RecipesAsMaterial ?? (_RecipesAsMaterial = BuildRecipesAsMaterial()); }
        }

        public IEnumerable<Recipe> RecipesAsResult {
            get { return _RecipesAsResult ?? (_RecipesAsResult = BuildRecipesAsResult()); }
        }

        public IEnumerable<IShopListing> AsShopItems {
            get { return _AsShopItems ?? (_AsShopItems = BuildAsShopItems()); }
        }

        public IEnumerable<IShopListingItem> AsShopPayment {
            get { return _AsShopPayment ?? (_AsShopPayment = BuildAsShopPayment()); }
        }

        #endregion

        #region Constructors

        #region Constructor

        public Item(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        private Recipe[] BuildRecipesAsMaterial() {
            var rSheet = Sheet.Collection.GetSheet<Recipe>();

            return
                rSheet.Where(
                             recipe =>
                             recipe.Ingredients.Any(ing => ing.Type == RecipeIngredientType.Material && ing.Item == this))
                      .ToArray();
        }

        private Recipe[] BuildRecipesAsResult() {
            var rSheet = Sheet.Collection.GetSheet<Recipe>();

            return rSheet.Where(recipe => recipe.ResultItem == this).ToArray();
        }

        private IShopListing[] BuildAsShopItems() {
            var shops = Sheet.Collection.Shops;

            var shopItems = new List<IShopListing>();
            foreach (var shop in shops)
                shopItems.AddRange(shop.ShopListings.Where(l => l.Rewards.Any(li => li.Item == this)));
            return shopItems.Distinct().ToArray();
        }

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

        #endregion
    }
}
