namespace SaintCoinach.Xiv {
    public interface IShopListingItem {
        #region Properties

        Item Item { get; }
        int Count { get; }
        bool IsHq { get; }
        IShopListing ShopItem { get; }

        #endregion
    }
}
