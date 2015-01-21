using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Shop : XivRow, IShop {
        #region Fields
        private ShopItem[] _ShopItems;
        private ENpc[] _ENpcs;
        #endregion

        #region Properties
        public string Name { get { return AsString("Name"); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        public BeastTribe BeastTribe { get { return As<BeastTribe>(); } }
        public BeastReputationRank BeastReputationRank { get { return As<BeastReputationRank>(); } }
        public Quest Quest { get { return As<Quest>(); } }
        public IEnumerable<ENpc> ENpcs { get { return _ENpcs ?? (_ENpcs = BuildENpcs()); } }
        public IEnumerable<ShopItem> Items { get { return _ShopItems ?? (_ShopItems = BuildShopItems()); } }
        #endregion

        #region Constructor
        public Shop(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }

        #region Build
        private ENpc[] BuildENpcs() {
            return Sheet.Collection.ENpcs.FindWithData(this.Key).ToArray();
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

        #region IShop Members

        IEnumerable<IShopListing> IShop.ShopListings {
            get { return Items.Cast<IShopListing>(); }
        }

        #endregion
    }
}