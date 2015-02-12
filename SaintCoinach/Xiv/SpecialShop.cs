using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class SpecialShop : XivRow, IShop {
        #region Fields

        private ENpc[] _ENpcs;
        private SpecialShopListing[] _ShopItems;

        #endregion

        #region Properties

        public IEnumerable<SpecialShopListing> Items { get { return _ShopItems ?? (_ShopItems = BuildShopItems()); } }

        #endregion

        #region Constructors

        #region Constructor

        public SpecialShop(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

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

        private SpecialShopListing[] BuildShopItems() {
            const int Count = 160;

            var items = new List<SpecialShopListing>();
            for (var i = 0; i < Count; ++i) {
                var item = new SpecialShopListing(this, i);
                if (item.Rewards.Any())
                    items.Add(item);
            }

            return items.ToArray();
        }

        #endregion
    }
}
