using System.Collections.Generic;

namespace SaintCoinach.Xiv {
    public interface IShopListing {
        #region Properties

        IEnumerable<IShopListingItem> Rewards { get; }
        IEnumerable<IShopListingItem> Costs { get; }
        IEnumerable<IShop> Shops { get; }

        #endregion
    }
}
