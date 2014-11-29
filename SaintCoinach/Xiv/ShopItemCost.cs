using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ShopItemCost : IShopItemCost {
        #region Fields
        private IShopItem _ShopItem;
        private Item _Item;
        private int _Count;
        private bool _RequireHq;
        #endregion

        #region Properties
        public IShopItem ShopItem { get { return _ShopItem; } }
        public Item Item {
            get { return _Item; }
        }
        public int Count {
            get { return _Count; }
        }
        public bool RequireHq {
            get { return _RequireHq; }
        }
        #endregion

        #region Constructor
        public ShopItemCost(IShopItem shopItem, Item item, int count, bool requireHq) {
            _ShopItem = shopItem;
            _Item = item;
            _Count = count;
            _RequireHq = requireHq;
        }
        #endregion

        public override string ToString() {
            var sb = new StringBuilder();

            if (Count > 1)
                sb.AppendFormat("{0} ", Count);
            sb.Append(Item);
            if (RequireHq)
                sb.Append(" (HQ)");
            return sb.ToString();
        }
    }
}
