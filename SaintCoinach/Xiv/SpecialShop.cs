using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class SpecialShop : XivRow, IShop {
        #region Fields
        private ENpc[] _ENpcs;
        private SpecialShopItem[] _ShopItems;
        #endregion

        #region Properties
        public string Name { get { return AsString("Name"); } }
        public IEnumerable<ENpc> ENpcs { get { return _ENpcs ?? (_ENpcs = BuildENpcs()); } }
        public IEnumerable<SpecialShopItem> Items { get { return _ShopItems ?? (_ShopItems = BuildShopItems()); } }
        #endregion

        #region Constructor
        public SpecialShop(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private ENpc[] BuildENpcs() {
            return Sheet.Collection.ENpcs.FindWithData(this.Key).ToArray();
        }
        private SpecialShopItem[] BuildShopItems() {
            const int Count = 160;

            var items = new List<SpecialShopItem>();
            for (var i = 0; i < Count; ++i) {
                var item = new SpecialShopItem(this, i);
                if (item.Item.Key != 0 && item.Count != 0)
                    items.Add(item);
            }

            return items.ToArray();
        }
        #endregion

        public override string ToString() {
            return Name;
        }

        #region IShop Members
        IEnumerable<IShopItem> IShop.ShopItems {
            get { return Items.Cast<IShopItem>(); }
        }
        #endregion
    }
}