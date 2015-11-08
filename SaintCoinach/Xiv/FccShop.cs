using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class FccShop : XivRow, IShop {
        #region Static
        
        private ENpc[] _ENpcs;
        private IShopListing[] _ShopListings;
        private Item[] _Items;

        #endregion

        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        string IShop.Name { get { return Name.ToString(); } }

        public IEnumerable<ENpc> ENpcs {
            get {
                return _ENpcs ?? (_ENpcs = BuildENpcs());
            }
        }

        public IEnumerable<IShopListing> ShopListings {
            get {
                return _ShopListings ?? (_ShopListings = BuildShopListings());
            }
        }
        IEnumerable<Item> IItemSource.Items {
            get {
                return _Items ?? (_Items = BuildItems());
            }
        }

        #endregion

        #region Constructors

        public FccShop(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build
        private ENpc[] BuildENpcs() {
            return Sheet.Collection.ENpcs.FindWithData(Key).ToArray();
        }

        const int ItemCount = 10;
        private IShopListing[] BuildShopListings() {
            const int CostItem = 6559;  // TODO: This is the company chest. Because there's no item for FC credit. :(

            var costItem = Sheet.Collection.GetSheet<Item>()[CostItem];
            var listings = new List<IShopListing>();
            for(var i = 0; i < ItemCount; ++i) {
                var item = As<Item>("Item", i);
                if (item == null || item.Key == 0)
                    continue;

                var cost = AsInt32("Cost", i);
                var requiredRank = As<FCRank>("FCRank{Required}", i);

                listings.Add(new Listing(this, item, costItem, cost, requiredRank));
            }
            return listings.ToArray();
        }

        private Item[] BuildItems() {
            var items = new List<Item>();
            for(var i = 0; i < ItemCount; ++i) {
                var item = As<Item>("Item", i);
                if (item != null && item.Key != 0)
                    items.Add(item);
            }
            return items.ToArray();
        }

        #endregion

        public class Listing : IShopListing {
            #region Fields
            IShopListingItem _Cost;
            IShopListingItem _Reward;
            IShop _Shop;
            #endregion

            public Listing(FccShop shop, Item rewardItem, Item costItem, int costCount, FCRank requiredFcRank) {
                _Shop = shop;
                _Cost = new ShopListingItem(this, costItem, costCount, false);
                _Reward = new ShopListingItem(this, rewardItem, 1, false);
            }

            public IEnumerable<IShopListingItem> Costs {
                get {
                    yield return _Cost;
                }
            }

            public IEnumerable<IShopListingItem> Rewards {
                get {
                    yield return _Reward;
                }
            }

            public IEnumerable<IShop> Shops {
                get {
                    yield return _Shop;
                }
            }
        }

        public override string ToString() {
            return Name;
        }
    }
}
