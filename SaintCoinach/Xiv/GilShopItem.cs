using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing an <see cref="Item" /> used in <see cref="GilShop" />s.
    /// </summary>
    public class GilShopItem : XivSubRow, IShopListing, IShopListingItem {
        #region Static

        /// <summary>
        ///     Key of the <see cref="Item" /> used as currency (Gil).
        /// </summary>
        private const int GilItemKey = 1;

        #endregion

        #region Fields

        /// <summary>
        ///     Cost of the current shop item.
        /// </summary>
        private readonly ShopListingItem _Cost;

        /// <summary>
        ///     Shops offering the current item.
        /// </summary>
        private GilShop[] _Shops;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the associated <see cref="Item" />.
        /// </summary>
        /// <value>The associated <see cref="Item" />.</value>
        public Item Item { get { return As<Item>(); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShopItem" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public GilShopItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) {
            _Cost = new ShopListingItem(this, Sheet.Collection.GetSheet<Item>()[GilItemKey],
                Item.Ask, false, 0);
        }

        #endregion

        /// <summary>
        ///     Returns a string representation of the current shop item.
        /// </summary>
        /// <returns>The name of <see cref="Item" />.</returns>
        public override string ToString() {
            return string.Format("{0}", Item);
        }

        #region Build

        /// <summary>
        ///     Build an array of the <see cref="GilShop" />s offering the current item.
        /// </summary>
        /// <returns>An array of the <see cref="GilShop" />s offering the current item.</returns>
        private GilShop[] BuildShops() {
            var sSheet = Sheet.Collection.GetSheet<GilShop>();
            return sSheet.Where(shop => shop.Items.Contains(this)).ToArray();
        }

        #endregion

        #region IShopListing Members

        /// <summary>
        ///     Gets the rewards of the current listing.
        /// </summary>
        /// <value>The rewards of the current listing.</value>
        IEnumerable<IShopListingItem> IShopListing.Rewards { get { yield return this; } }

        /// <summary>
        ///     Gets the costs of the current listing.
        /// </summary>
        /// <value>The costs of the current listing.</value>
        IEnumerable<IShopListingItem> IShopListing.Costs { get { yield return _Cost; } }

        /// <summary>
        ///     Gets the shops offering the current listing.
        /// </summary>
        /// <value>The shops offering the current listing.</value>
        public IEnumerable<IShop> Shops { get { return _Shops ?? (_Shops = BuildShops()); } }

        #endregion

        #region IShopListingItem Members

        /// <summary>
        ///     Gets the item of the current listing entry.
        /// </summary>
        /// <value>The item of the current listing entry.</value>
        Item IShopListingItem.Item { get { return Item; } }

        /// <summary>
        ///     Gets the count for the current listing entry.
        /// </summary>
        /// <value>
        ///     <value>1</value>
        /// </value>
        int IShopListingItem.Count { get { return 1; } }

        /// <summary>
        ///     Gets a value indicating whether the item is high-quality.
        /// </summary>
        /// <value>
        ///     <c>false</c>
        /// </value>
        bool IShopListingItem.IsHq { get { return false; } }

        /// <summary>
        ///     Gets the collectability rating for the item.
        /// </summary>
        /// <value>
        ///     <c>false</c>
        /// </value>
        int IShopListingItem.CollectabilityRating { get { return 0; } }

        /// <summary>
        ///     Gets the <see cref="IShopListing" /> the current entry is for.
        /// </summary>
        /// <value>
        ///     <c>this</c>
        /// </value>
        IShopListing IShopListingItem.ShopItem { get { return this; } }

        #endregion
    }
}
