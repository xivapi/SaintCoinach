using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class GrandCompanySealShopItem : XivRow, IShopListing, IShopListingItem {
        #region Properties

        // ReSharper disable once InconsistentNaming
        public GCShop GCShop { get; private set; }
        public ShopListingItem Cost { get; private set; }
        public GrandCompanyRank GrandCompanyRank { get { return As<GrandCompanyRank>(); } }
        // ReSharper disable once InconsistentNaming
        public GCShopItemCategory GCShopItemCategory { get { return As<GCShopItemCategory>(); } }

        #endregion

        #region Constructors

        #region Constructor

        public GrandCompanySealShopItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) {
            GCShop = Sheet.Collection.GetSheet<GCShop>().FirstOrDefault(_ => _.Min <= Key && _.Max >= Key);

            if (GCShop == null) return;

            var sealItem = GCShop.GrandCompany.SealItem;
            Cost = new ShopListingItem(this, sealItem, AsInt32("Cost"), false);
        }

        #endregion

        #endregion

        public Item Item { get { return As<Item>("Item"); } }
        public int Count { get { return AsInt32("Count"); } }

        public override string ToString() {
            return string.Format("{0}", Item);
        }

        #region IShopListing Members

        IEnumerable<IShopListingItem> IShopListing.Rewards { get { yield return this; } }

        IEnumerable<IShopListingItem> IShopListing.Costs { get { yield return Cost; } }

        IEnumerable<IShop> IShopListing.Shops { get { yield return GCShop; } }

        #endregion

        #region IShopListingItem Members

        ItemBase IShopListingItem.Item { get { return Item; } }

        bool IShopListingItem.IsHq { get { return false; } }

        IShopListing IShopListingItem.ShopItem { get { return this; } }

        #endregion
    }
}
