using System.Collections.Generic;

namespace SaintCoinach.Xiv {
    public interface IShop {
        #region Properties

        int Key { get; }
        string Name { get; }
        IEnumerable<ENpc> ENpcs { get; }
        IEnumerable<IShopListing> ShopListings { get; }

        #endregion
    }
}
