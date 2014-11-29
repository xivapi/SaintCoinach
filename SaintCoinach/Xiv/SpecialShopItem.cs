using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class SpecialShopItem : IShopItem {
        #region Fields
        private SpecialShop _SpecialShop;
        private ShopItemCost[] _Costs;
        private Item _Item;
        private Quest _Quest;
        private int _Count;
        #endregion

        #region Properties
        public Item Item { get { return _Item; } }
        public int Count { get { return _Count; } }
        public IEnumerable<IShopItemCost> Costs { get { return _Costs; } }
        public SpecialShop SpecialShop { get { return _SpecialShop; } }
        public Quest Quest { get { return _Quest; } }
        #endregion

        #region Constructor
        public SpecialShopItem(SpecialShop shop, int index) {
            _SpecialShop = shop;

            _Item = shop.As<Item>("Item{Receive}", index);
            _Count = shop.AsInt32("Count{Receive}", index);
            _Quest = shop.As<Quest>("Quest{Item}", index);

            const int CostCount = 3;
            var costs = new List<ShopItemCost>();
            for (var i = 0; i < CostCount; ++i) {
                var item = shop.As<Item>("Item{Cost}", index, i);
                if (item.Key == 0)
                    continue;

                var count = shop.AsInt32("Count{Cost}", index, i);
                if (count == 0)
                    continue;

                var hq = shop.AsBoolean("HQ{Cost}", index, i);

                costs.Add(new ShopItemCost(this, item, count, hq));
            }
            _Costs = costs.ToArray();
        }
        #endregion

        #region IShopItem Members
        IEnumerable<IShop> IShopItem.Shops {
            get { yield return SpecialShop; }
        }

        #endregion
    }
}
