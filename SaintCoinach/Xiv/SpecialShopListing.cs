using System.Collections.Generic;

namespace SaintCoinach.Xiv {
    public class SpecialShopListing : IShopListing {
        #region Fields

        private readonly ShopListingItem[] _Costs;
        private readonly ShopListingItem[] _Rewards;

        #endregion

        #region Properties

        public SpecialShop SpecialShop { get; private set; }
        public Quest Quest { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public SpecialShopListing(SpecialShop shop, int index) {
            SpecialShop = shop;

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
            Quest = shop.As<Quest>("Quest{Item}", index);

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

        #endregion

        public IEnumerable<IShopListingItem> Rewards { get { return _Rewards; } }
        public IEnumerable<IShopListingItem> Costs { get { return _Costs; } }

        #region IShopItem Members

        IEnumerable<IShop> IShopListing.Shops { get { yield return SpecialShop; } }

        #endregion
    }
}
