using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class GrandCompanySealShopItem : XivRow, IShopListing, IShopListingItem {
        #region Fields
        private GCShop _GCShop;
        private ShopListingItem _Cost;
        #endregion

        #region Properties
        public Item Item { get { return As<Item>("Item"); } }
        public int Count { get { return AsInt32("Count"); } }
        public GCShop GCShop { get { return _GCShop; } }
        public ShopListingItem Cost { get { return _Cost; } }
        public GrandCompanyRank GrandCompanyRank { get { return As<GrandCompanyRank>(); } }
        public GCShopItemCategory GCShopItemCategory { get { return As<GCShopItemCategory>(); } }
        #endregion

        #region Constructor
        public GrandCompanySealShopItem(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) {
            _GCShop = Sheet.Collection.GetSheet<GCShop>().FirstOrDefault(_ => _.Min <= Key && _.Max >= Key);

            if (_GCShop != null) {
                var sealItem = _GCShop.GrandCompany.SealItem;
                _Cost = new ShopListingItem(this, sealItem, AsInt32("Cost"), false);
            }
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
            get { yield return _Cost; }
        }

        IEnumerable<IShop> IShopListing.Shops {
            get { yield return _GCShop; }
        }

        #endregion

        #region IShopListingItem Members
        bool IShopListingItem.IsHq {
            get { return false; }
        }

        IShopListing IShopListingItem.ShopItem {
            get { return this; }
        }

        #endregion
    }
}