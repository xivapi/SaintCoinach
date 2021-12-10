using System.Collections.Generic;
using System.Linq;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a listing in a <see cref="SpecialShop" />.
    /// </summary>
    public class SpecialShopListing : IShopListing {
        #region Currency

        private static Dictionary<int, int> _Currencies = new Dictionary<int, int>() {
            { 1, 28 },
            { 2, 25199 },
            { 4, 25200 },
            { 6, 33913 },
            { 7, 33914 }
        };

        private static Dictionary<int, int> _Tomestones;

        private void BuildTomestones() {
            // Tomestone currencies rotate across patches.
            // These keys correspond to currencies A, B, and C.
            var sTomestonesItems = SpecialShop.Sheet.Collection.GetSheet<TomestonesItem>()
                .Where(t => t.Tomestone.Key > 0)
                .OrderBy(t => t.Tomestone.Key)
                .ToArray();

            _Tomestones = new Dictionary<int, int>();

            for (int i = 0; i < sTomestonesItems.Length; i++) {
                _Tomestones[i + 1] = sTomestonesItems[i].Item.Key;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        ///     Costs of the current listing.
        /// </summary>
        private readonly ShopListingItem[] _Costs;

        /// <summary>
        ///     Rewards of the current listing.
        /// </summary>
        private readonly ShopListingItem[] _Rewards;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the <see cref="SpecialShop" /> the current listing is from.
        /// </summary>
        /// <value>The <see cref="SpecialShop" /> the current listing is from.</value>
        public SpecialShop SpecialShop { get; private set; }

        /// <summary>
        ///     Gets the <see cref="Quest" /> required for the current listing.
        /// </summary>
        /// <value>The <see cref="Quest" /> required for the current listing.</value>
        public Quest Quest { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpecialShopListing" /> class.
        /// </summary>
        /// <param name="shop"><see cref="SpecialShop" /> for which the listing is.</param>
        /// <param name="index">Position of the listing in the <c>shop</c>'s data.</param>
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

                rewards.Add(new ShopListingItem(this, item, count, hq, 0));
            }
            _Rewards = rewards.ToArray();
            Quest = shop.As<Quest>("Quest{Item}", index);

            int UseCurrencyType = shop.As<byte>("UseCurrencyType");

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

                if (item.Key < 8) {
                    switch (UseCurrencyType) {
                        case 16:
                            item = shop.Sheet.Collection.GetSheet<Item>()
                                [_Currencies[item.Key]];
                            break;
                        case 8:
                            item = shop.Sheet.Collection.GetSheet<Item>()
                                [1];
                            break;
                        case 4:
                            if (_Tomestones == null) {
                                BuildTomestones();
                            }
                            item = shop.Sheet.Collection.GetSheet<Item>()
                                [_Tomestones[item.Key]];
                            break;
                    }
                    hq = false;
                }
                
                var collectabilityRating = shop.AsInt16("CollectabilityRating{Cost}", index, i);

                costs.Add(new ShopListingItem(this, item, count, hq, collectabilityRating));
            }
            _Costs = costs.ToArray();
        }

        #endregion

        /// <summary>
        ///     Gets the rewards of the current listing.
        /// </summary>
        /// <value>The rewards of the current listing.</value>
        public IEnumerable<IShopListingItem> Rewards { get { return _Rewards; } }

        /// <summary>
        ///     Gets the costs of the current listing.
        /// </summary>
        /// <value>The costs of the current listing.</value>
        public IEnumerable<IShopListingItem> Costs { get { return _Costs; } }

        #region IShopItem Members

        /// <summary>
        ///     Gets the shops offering the current listing.
        /// </summary>
        /// <value>The shops offering the current listing.</value>
        IEnumerable<IShop> IShopListing.Shops { get { yield return SpecialShop; } }

        #endregion
    }
}
