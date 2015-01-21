using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class SpecialShop : XivRow, IShop {
        #region Fields
        private ENpc[] _ENpcs;
        private SpecialShopListing[] _ShopItems;
        #endregion

        #region Properties
        public string Name { get { return AsString("Name"); } }
        public IEnumerable<ENpc> ENpcs { get { return _ENpcs ?? (_ENpcs = BuildENpcs()); } }
        public IEnumerable<SpecialShopListing> Items { get { return _ShopItems ?? (_ShopItems = BuildShopItems()); } }
        #endregion

        #region Constructor
        public SpecialShop(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private ENpc[] BuildENpcs() {
            return Sheet.Collection.ENpcs.FindWithData(this.Key).ToArray();
        }
        private SpecialShopListing[] BuildShopItems() {
            const int Count = 160;

            var items = new List<SpecialShopListing>();
            for (var i = 0; i < Count; ++i) {
                var item = new SpecialShopListing(this, i);
                if(item.Rewards.Any())
                    items.Add(item);
            }

            return items.ToArray();
        }
        #endregion

        public override string ToString() {
            return Name;
        }

        #region IShop Members
        IEnumerable<IShopListing> IShop.ShopListings {
            get { return Items.Cast<IShopListing>(); }
        }
        #endregion
    }
}