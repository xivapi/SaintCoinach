using System.Text;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     General-purpose class for items to use in <see cref="IShopListing" />.
    /// </summary>
    public class ShopListingItem : IShopListingItem {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShopListingItem" /> class.
        /// </summary>
        /// <param name="shopItem">The <see cref="IShopListing" /> the entry is for.</param>
        /// <param name="item">The item of the entry.</param>
        /// <param name="count">The count for the entry.</param>
        /// <param name="isHq">A value indicating whether the <c>item</c> is high-quality.</param>
        public ShopListingItem(IShopListing shopItem, ItemBase item, int count, bool isHq) {
            ShopItem = shopItem;
            Item = item;
            Count = count;
            IsHq = isHq;
        }

        #endregion

        /// <summary>
        ///     Gets the <see cref="IShopListing" /> the current entry is for.
        /// </summary>
        /// <value>The <see cref="IShopListing" /> the current entry is for.</value>
        public IShopListing ShopItem { get; private set; }

        /// <summary>
        ///     Gets the item of the current listing entry.
        /// </summary>
        /// <value>The item of the current listing entry.</value>
        public ItemBase Item { get; private set; }

        /// <summary>
        ///     Gets the count for the current listing entry.
        /// </summary>
        /// <value>The count for the current listing entry.</value>
        public int Count { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the item is high-quality.
        /// </summary>
        /// <value>A value indicating whether the item is high-quality.</value>
        public bool IsHq { get; private set; }

        /// <summary>
        ///     Returns a string that represents the current <see cref="ShopListingItem" />.
        /// </summary>
        /// <returns>A string that represents the current <see cref="ShopListingItem" />.</returns>
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
