using System.Collections.Generic;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Interface for shops.
    /// </summary>
    public interface IShop : IItemSource {
        #region Properties

        /// <summary>
        ///     Gets the key of the current shop.
        /// </summary>
        /// <value>The key of the current shop.</value>
        int Key { get; }

        /// <summary>
        ///     Gets the name of the current shop.
        /// </summary>
        /// <value>The name of the current shop.</value>
        string Name { get; }

        /// <summary>
        ///     Gets the <see cref="ENpcs" /> offering the current shop.
        /// </summary>
        /// <value>The <see cref="ENpcs" /> offering the current shop.</value>
        IEnumerable<ENpc> ENpcs { get; }

        /// <summary>
        ///     Gets the listings of the current shop.
        /// </summary>
        /// <value>The listings of the current shop.</value>
        IEnumerable<IShopListing> ShopListings { get; }

        #endregion
    }
}
