using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ShopItem : XivRow, IShopListing, IShopListingItem {
        const int GilItemKey = 1;

        #region Fields
        private Shop[] _Shops;
        private ShopListingItem _Cost;
        #endregion

        #region Properties
        public Item Item { get { return As<Item>(); } }
        public Quest Quest { get { return As<Quest>(); } }
        public int PriceFactor { get { return AsInt32("PriceFactor{Mid}"); } }
        #endregion

        #region Constructor
        public ShopItem(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) {
            _Cost = new ShopListingItem(this, Sheet.Collection.GetSheet<Item>()[GilItemKey], (PriceFactor * ((InventoryItem)Item).Ask) / 100, false);
        }
        #endregion

        public override string ToString() {
            return string.Format("{0}", Item);
        }

        #region IShopListing Members
        IEnumerable<IShopListingItem> IShopListing.Rewards {
            get { yield return this; }
        }

        IEnumerable<IShopListingItem> IShopListing.Costs {
            get { yield return (IShopListingItem)_Cost; }
        }

        public IEnumerable<IShop> Shops {
            get { return _Shops ?? (_Shops = BuildShops()); }
        }

        #endregion

        #region Build
        private Shop[] BuildShops() {
            var sSheet = Sheet.Collection.GetSheet<Shop>();
            return sSheet.Where(shop => shop.Items.Contains(this)).ToArray();
        }
        #endregion

        #region IShopListingItem Members
        int IShopListingItem.Count {
            get { return 1; }
        }

        bool IShopListingItem.IsHq {
            get { return false; }
        }

        IShopListing IShopListingItem.ShopItem {
            get { return this; }
        }

        #endregion
    }
}