using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class GCShop : XivRow, IShop {
        const int GrandCompanyKeyOffset = 1441792;

        #region Fields
        private ENpc[] _ENpcs;
        private GrandCompany _GrandCompany;
        private GrandCompanySealShopItem[] _Items;
        #endregion

        #region Properties
        public IEnumerable<ENpc> ENpcs { get { return _ENpcs ?? (_ENpcs = BuildENpcs()); } }
        public int Min { get { return AsInt32("Min"); } }
        public int Max { get { return AsInt32("Max"); } }
        public GrandCompany GrandCompany {
            get {
                if (_GrandCompany == null)
                    _GrandCompany = Sheet.Collection.GetSheet<GrandCompany>()[Key - GrandCompanyKeyOffset];
                return _GrandCompany;
            }
        }
        public IEnumerable<GrandCompanySealShopItem> Items { get { return _Items ?? (_Items = BuildItems()); } }
        IEnumerable<IShopListing> IShop.ShopListings { get { return Items.Cast<IShopListing>(); } }
        string IShop.Name { get { return string.Format("{0}", GrandCompany); } }
        #endregion

        #region Constructor
        public GCShop(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private ENpc[] BuildENpcs() {
            return Sheet.Collection.ENpcs.FindWithData(this.Key).ToArray();
        }
        private GrandCompanySealShopItem[] BuildItems() {
            var items = new List<GrandCompanySealShopItem>();

            var gcItems = Sheet.Collection.GetSheet<GrandCompanySealShopItem>();
            for (var i = Min; i <= Max; ++i) {
                var item = gcItems[i];
                if (item.Item.Key != 0)
                    items.Add(item);
            }

            return items.ToArray();
        }
        #endregion
    }
}