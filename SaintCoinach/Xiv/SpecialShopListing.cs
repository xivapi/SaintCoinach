using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class SpecialShopListing : IShopListing {
        #region Fields
        private SpecialShop _SpecialShop;
        private ShopListingItem[] _Rewards;
        private ShopListingItem[] _Costs;
        private Item _Item;
        private Quest _Quest;
        private int _Count;
        #endregion

        #region Properties
        public IEnumerable<IShopListingItem> Rewards { get { return _Rewards; } }
        public IEnumerable<IShopListingItem> Costs { get { return _Costs; } }
        public SpecialShop SpecialShop { get { return _SpecialShop; } }
        public Quest Quest { get { return _Quest; } }
        #endregion

        #region Constructor
        public SpecialShopListing(SpecialShop shop, int index) {
            _SpecialShop = shop;

            const int RewardCount = 2;
            var rewards = new List<ShopListingItem>();
            for (var i = 0; i < RewardCount; ++i) {
                var item = shop.As<Item>("Item{Receive}", index, i);
                if (item.Key == 0)
                    continue;

                var count = shop.AsInt32("Count{Receive}", index, i);
                if (count == 0)
                    continue;

                var hq = shop.AsBoolean("HQ{Receive}", index, i);

                rewards.Add(new ShopListingItem(this, item, count, hq));
            }
            _Rewards = rewards.ToArray();
            _Quest = shop.As<Quest>("Quest{Item}", index);

            const int CostCount = 3;
            var costs = new List<ShopListingItem>();
            for (var i = 0; i < CostCount; ++i) {
                var item = shop.As<Item>("Item{Cost}", index, i);
                if (item.Key == 0)
                    continue;

                var count = shop.AsInt32("Count{Cost}", index, i);
                if (count == 0)
                    continue;

                var hq = shop.AsBoolean("HQ{Cost}", index, i);

                costs.Add(new ShopListingItem(this, item, count, hq));
            }
            _Costs = costs.ToArray();
        }
        #endregion

        #region IShopItem Members
        IEnumerable<IShop> IShopListing.Shops {
            get { yield return SpecialShop; }
        }

        #endregion
    }
}
