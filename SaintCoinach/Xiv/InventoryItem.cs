using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class InventoryItem : Item {
        const double BidFactor = 0.05;

        #region Fields
        private Recipe[] _RecipesAsMaterial = null;
        private Recipe[] _RecipesAsResult = null;
        private IShopItem[] _AsShopItems = null;
        private IShopItemCost[] _AsShopPayment = null;
        #endregion

        #region Properties
        public int ItemLeve { get { return AsInt32("Level{Item}"); } }
        public bool IsUnique { get { return AsBoolean("IsUnique"); } }
        public bool IsUntradable { get { return AsBoolean("IsUntradable"); } }
        public bool IsIndisposable { get { return AsBoolean("IsIndisposable"); } }
        public bool IsDyeable { get { return AsBoolean("IsDyeable"); } }
        public Stain Stain { get { return As<Stain>(); } }
        public ItemUICategory ItemUICategory { get { return As<ItemUICategory>(); } }
        public ItemSearchCategory ItemSearchCategory { get { return As<ItemSearchCategory>(); } }
        public ItemAction ItemAction { get { return As<ItemAction>(); } }

        public int Bid { get { return (int)Math.Max(1, Math.Round(Ask * BidFactor)); } }
        public int Ask { get { return AsInt32("Price{Mid}"); } }

        public IEnumerable<Recipe> RecipesAsMaterial { get { return _RecipesAsMaterial ?? (_RecipesAsMaterial = BuildRecipesAsMaterial()); } }
        public IEnumerable<Recipe> RecipesAsResult { get { return _RecipesAsResult ?? (_RecipesAsResult = BuildRecipesAsResult()); } }
        public IEnumerable<IShopItem> AsShopItems { get { return _AsShopItems ?? (_AsShopItems = BuildAsShopItems()); } }
        public IEnumerable<IShopItemCost> AsShopPayment { get { return _AsShopPayment ?? (_AsShopPayment = BuildAsShopPayment()); } }
        #endregion

        #region Constructor
        public InventoryItem(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private Recipe[] BuildRecipesAsMaterial() {
            var rSheet = Sheet.Collection.GetSheet<Recipe>();

            return rSheet.Where(recipe => recipe.Ingredients.Any(ing => ing.Type == RecipeIngredientType.Material && ing.Item == this)).ToArray();
        }
        private Recipe[] BuildRecipesAsResult() {
            var rSheet = Sheet.Collection.GetSheet<Recipe>();

            return rSheet.Where(recipe => recipe.ResultItem == this).ToArray();
        }
        private IShopItem[] BuildAsShopItems() {
            var shops = Sheet.Collection.Shops;

            var shopItems = new List<IShopItem>();
            foreach (var shop in shops)
                shopItems.AddRange(shop.ShopItems.Where(_ => _.Item == this));
            return shopItems.ToArray();
        }
        private IShopItemCost[] BuildAsShopPayment() {
            if (Key == 1)
                return new IShopItemCost[0];    // XXX: DO NOT BUILD THIS FOR GIL, THAT WOULD BE BAD.

            var shops = Sheet.Collection.Shops;

            var checkedItems = new List<IShopItem>();
            var shopItemCosts = new List<IShopItemCost>();
            foreach (var shop in shops) {
                foreach (var item in shop.ShopItems.Except(checkedItems).ToArray()) {
                    shopItemCosts.AddRange(item.Costs.Where(_ => _.Item == this));
                    checkedItems.Add(item);
                }
            }
            return shopItemCosts.ToArray();
        }
        #endregion
    }
}