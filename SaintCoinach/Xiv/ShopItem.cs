using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing an <see cref="Item" /> used in <see cref="Shop" />s.
    /// </summary>
    public class ShopItem : XivRow, IShopListing, IShopListingItem {
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
        private Shop[] _Shops;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the <see cref="Quest" /> required to purchase the current item.
        /// </summary>
        /// <value>The <see cref="Quest" /> required to purchase the current item.</value>
        public Quest Quest { get { return As<Quest>(); } }

        /// <summary>
        ///     Gets the factor, in %, applied to the <see cref="Item" />'s base price.
        /// </summary>
        /// <value>The factor, in %, applied to the <see cref="Item" />'s base price.</value>
        public int PriceFactor { get { return AsInt32("PriceFactor{Mid}"); } }

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
        public ShopItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) {
            _Cost = new ShopListingItem(this, Sheet.Collection.GetSheet<Item>()[GilItemKey],
                (PriceFactor * Item.Ask) / 100, false);
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
        ///     Build an array of the <see cref="Shop" />s offering the current item.
        /// </summary>
        /// <returns>An array of the <see cref="Shop" />s offering the current item.</returns>
        private Shop[] BuildShops() {
            var sSheet = Sheet.Collection.GetSheet<Shop>();
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
        ItemBase IShopListingItem.Item { get { return Item; } }

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
        ///     Gets the <see cref="IShopListing" /> the current entry is for.
        /// </summary>
        /// <value>
        ///     <c>this</c>
        /// </value>
        IShopListing IShopListingItem.ShopItem { get { return this; } }

        #endregion
    }
}
