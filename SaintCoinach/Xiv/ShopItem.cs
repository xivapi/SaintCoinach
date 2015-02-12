using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ShopItem : XivRow, IShopListing, IShopListingItem {
        #region Static

        private const int GilItemKey = 1;

        #endregion

        #region Fields

        private readonly ShopListingItem _Cost;
        private Shop[] _Shops;

        #endregion

        #region Properties

        public Quest Quest { get { return As<Quest>(); } }
        public int PriceFactor { get { return AsInt32("PriceFactor{Mid}"); } }

        #endregion

        #region Constructors

        #region Constructor

        public ShopItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) {
            _Cost = new ShopListingItem(this, Sheet.Collection.GetSheet<Item>()[GilItemKey],
                (PriceFactor * ((InventoryItem)Item).Ask) / 100, false);
        }

        #endregion

        #endregion

        public Item Item { get { return As<Item>(); } }

        public override string ToString() {
            return string.Format("{0}", Item);
        }

        #region Build

        private Shop[] BuildShops() {
            var sSheet = Sheet.Collection.GetSheet<Shop>();
            return sSheet.Where(shop => shop.Items.Contains(this)).ToArray();
        }

        #endregion

        #region IShopListing Members

        IEnumerable<IShopListingItem> IShopListing.Rewards { get { yield return this; } }

        IEnumerable<IShopListingItem> IShopListing.Costs { get { yield return _Cost; } }

        public IEnumerable<IShop> Shops { get { return _Shops ?? (_Shops = BuildShops()); } }

        #endregion

        #region IShopListingItem Members

        int IShopListingItem.Count { get { return 1; } }

        bool IShopListingItem.IsHq { get { return false; } }

        IShopListing IShopListingItem.ShopItem { get { return this; } }

        #endregion
    }
}
