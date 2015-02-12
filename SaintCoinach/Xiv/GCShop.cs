using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    // ReSharper disable once InconsistentNaming
    public class GCShop : XivRow, IShop {
        #region Static

        private const int GrandCompanyKeyOffset = 1441792;

        #endregion

        #region Fields

        private ENpc[] _ENpcs;
        private GrandCompany _GrandCompany;
        private GrandCompanySealShopItem[] _Items;

        #endregion

        #region Properties

        public int Min { get { return AsInt32("Min"); } }
        public int Max { get { return AsInt32("Max"); } }

        public GrandCompany GrandCompany {
            get {
                return _GrandCompany
                       ?? (_GrandCompany = Sheet.Collection.GetSheet<GrandCompany>()[Key - GrandCompanyKeyOffset]);
            }
        }

        public IEnumerable<GrandCompanySealShopItem> Items { get { return _Items ?? (_Items = BuildItems()); } }

        #endregion

        #region Constructors

        #region Constructor

        public GCShop(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public IEnumerable<ENpc> ENpcs { get { return _ENpcs ?? (_ENpcs = BuildENpcs()); } }
        IEnumerable<IShopListing> IShop.ShopListings { get { return Items; } }
        string IShop.Name { get { return string.Format("{0}", GrandCompany); } }

        #region Build

        private ENpc[] BuildENpcs() {
            return Sheet.Collection.ENpcs.FindWithData(Key).ToArray();
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
