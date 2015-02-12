using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class Shop : XivRow, IShop {
        #region Fields

        private ENpc[] _ENpcs;
        private ShopItem[] _ShopItems;

        #endregion

        #region Properties

        public ImageFile Icon { get { return AsImage("Icon"); } }
        public BeastTribe BeastTribe { get { return As<BeastTribe>(); } }
        public BeastReputationRank BeastReputationRank { get { return As<BeastReputationRank>(); } }
        public Quest Quest { get { return As<Quest>(); } }
        public IEnumerable<ShopItem> Items { get { return _ShopItems ?? (_ShopItems = BuildShopItems()); } }

        #endregion

        #region Constructors

        #region Constructor

        public Shop(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public string Name { get { return AsString("Name"); } }
        public IEnumerable<ENpc> ENpcs { get { return _ENpcs ?? (_ENpcs = BuildENpcs()); } }

        #region IShop Members

        IEnumerable<IShopListing> IShop.ShopListings { get { return Items; } }

        #endregion

        public override string ToString() {
            return Name;
        }

        #region Build

        private ENpc[] BuildENpcs() {
            return Sheet.Collection.ENpcs.FindWithData(Key).ToArray();
        }

        private ShopItem[] BuildShopItems() {
            const int Count = 40;

            var items = new List<ShopItem>();
            for (var i = 0; i < Count; ++i) {
                var item = As<ShopItem>(i);
                if (item.Key != 0)
                    items.Add(item);
            }

            return items.ToArray();
        }

        #endregion
    }
}
