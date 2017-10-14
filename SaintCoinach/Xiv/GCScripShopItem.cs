using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class GCScripShopItem : XivSubRow, IShopListing, IShopListingItem {
        #region Properties

        // ReSharper disable once InconsistentNaming
        public GCShop GCShop { get; private set; }
        public ShopListingItem Cost { get; private set; }
        // ReSharper disable once InconsistentNaming
        public GCScripShopCategory GCScripShopCategory { get; private set; }

        public Item Item => As<Item>();
        public GrandCompanyRank RequiredGrandCompanyRank => As<GrandCompanyRank>("Required{GrandCompanyRank}");
        public int GCSealsCost => AsInt32("Cost{GCSeals}");
        public byte SortKey => As<byte>("SortKey");

        #endregion

        #region Constructors

        #region Constructor

        public GCScripShopItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) {
            GCScripShopCategory = Sheet.Collection.GetSheet<GCScripShopCategory>()[ParentKey];
            GCShop = Sheet.Collection.GetSheet<GCShop>().FirstOrDefault(_ => _.GrandCompany.Key == GCScripShopCategory.GrandCompany.Key);

            var sealItem = GCShop.GrandCompany.SealItem;
            Cost = new ShopListingItem(this, sealItem, GCSealsCost, false, 0);
        }

        #endregion

        #endregion

        public override string ToString() => Item?.ToString();

        #region IShopListing Members

        IEnumerable<IShopListingItem> IShopListing.Rewards { get { yield return this; } }

        IEnumerable<IShopListingItem> IShopListing.Costs { get { yield return Cost; } }

        IEnumerable<IShop> IShopListing.Shops { get { yield return GCShop; } }

        #endregion

        #region IShopListingItem Members

        Item IShopListingItem.Item => Item;

        bool IShopListingItem.IsHq => false;

        IShopListing IShopListingItem.ShopItem => this;

        int IShopListingItem.CollectabilityRating => 0;

        int IShopListingItem.Count => 1;

        #endregion
    }
}
