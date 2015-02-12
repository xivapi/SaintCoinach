using System.Collections.Generic;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Interface for listings of shops.
    /// </summary>
    public interface IShopListing {
        #region Properties

        /// <summary>
        ///     Gets the rewards of the current listing.
        /// </summary>
        /// <value>The rewards of the current listing.</value>
        IEnumerable<IShopListingItem> Rewards { get; }

        /// <summary>
        ///     Gets the costs of the current listing.
        /// </summary>
        /// <value>The costs of the current listing.</value>
        IEnumerable<IShopListingItem> Costs { get; }

        /// <summary>
        ///     Gets the shops offering the current listing.
        /// </summary>
        /// <value>The shops offering the current listing.</value>
        IEnumerable<IShop> Shops { get; }

        #endregion
    }
}
