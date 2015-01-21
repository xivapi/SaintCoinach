using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ShopListingItem : IShopListingItem {
        #region Fields
        private IShopListing _ShopItem;
        private Item _Item;
        private int _Count;
        private bool _IsHq;
        #endregion

        #region Properties
        public IShopListing ShopItem { get { return _ShopItem; } }
        public Item Item {
            get { return _Item; }
        }
        public int Count {
            get { return _Count; }
        }
        public bool IsHq {
            get { return _IsHq; }
        }
        #endregion

        #region Constructor
        public ShopListingItem(IShopListing shopItem, Item item, int count, bool isHq) {
            _ShopItem = shopItem;
            _Item = item;
            _Count = count;
            _IsHq = isHq;
        }
        #endregion

        public override string ToString() {
            var sb = new StringBuilder();

            if (Count > 1)
                sb.AppendFormat("{0} ", Count);
            sb.Append(Item);
            if (IsHq)
                sb.Append(" (HQ)");
            return sb.ToString();
        }
    }
}
