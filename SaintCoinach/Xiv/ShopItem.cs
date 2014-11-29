using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ShopItem : XivRow, IShopItem {
        const int GilItemKey = 1;

        #region Fields
        private Shop[] _Shops;
        private ShopItemCost _Cost;
        #endregion

        #region Properties
        public Item Item { get { return As<Item>(); } }
        public Quest Quest { get { return As<Quest>(); } }
        public int PriceFactor { get { return AsInt32("PriceFactor{Mid}"); } }
        #endregion

        #region Constructor
        public ShopItem(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) {
            _Cost = new ShopItemCost(this, Sheet.Collection.GetSheet<Item>()[GilItemKey], (PriceFactor * ((InventoryItem)Item).Ask) / 100, false);
        }
        #endregion

        public override string ToString() {
            return string.Format("{0}", Item);
        }

        #region IShopItem Members
        int IShopItem.Count {
            get { return 1; }
        }

        IEnumerable<IShopItemCost> IShopItem.Costs {
            get { yield return (IShopItemCost)_Cost; }
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
    }
}