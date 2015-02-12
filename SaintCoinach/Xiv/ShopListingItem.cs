using System.Text;

namespace SaintCoinach.Xiv {
    public class ShopListingItem : IShopListingItem {
        #region Constructors

        #region Constructor

        public ShopListingItem(IShopListing shopItem, Item item, int count, bool isHq) {
            ShopItem = shopItem;
            Item = item;
            Count = count;
            IsHq = isHq;
        }

        #endregion

        #endregion

        public IShopListing ShopItem { get; private set; }
        public Item Item { get; private set; }
        public int Count { get; private set; }
        public bool IsHq { get; private set; }

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
