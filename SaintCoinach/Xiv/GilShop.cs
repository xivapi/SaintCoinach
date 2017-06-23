using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a shop that trades using gil.
    /// </summary>
    public class GilShop : XivRow, IShop, IItemSource {
        #region Fields

        /// <summary>
        ///     <see cref="ENpc" />s offering the current shop.
        /// </summary>
        private ENpc[] _ENpcs;

        /// <summary>
        ///     <see cref="ShopItem" />s offered by the current shop.
        /// </summary>
        private GilShopItem[] _ShopItems;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the <see cref="GilShopItem" />s offered by the current shop.
        /// </summary>
        /// <value>The <see cref="GilShopItem" />s offered by the current shop.</value>
        public IEnumerable<GilShopItem> Items { get { return _ShopItems ?? (_ShopItems = BuildShopItems()); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GilShop" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public GilShop(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        /// <summary>
        ///     Gets the name of the current shop.
        /// </summary>
        /// <value>The name of the current shop.</value>
        public Text.XivString Name { get { return AsString("Name"); } }

        /// <summary>
        ///     Gets the <see cref="ENpc" />s offering the current shop.
        /// </summary>
        /// <value>The <see cref="ENpc" />s offering the current shop.</value>
        public IEnumerable<ENpc> ENpcs { get { return _ENpcs ?? (_ENpcs = BuildENpcs()); } }

        #region IShop Members

        /// <summary>
        ///     Gets the listings of the current shop.
        /// </summary>
        /// <value>The listings of the current shop.</value>
        IEnumerable<IShopListing> IShop.ShopListings { get { return Items; } }

        #endregion

        /// <summary>
        ///     Returns a string representation of the current shop.
        /// </summary>
        /// <returns>The value of <see cref="Name" />.</returns>
        public override string ToString() {
            return Name;
        }

        #region Build

        /// <summary>
        ///     Build an array of the <see cref="ENpc" />s offering the current shop.
        /// </summary>
        /// <returns>An array of the <see cref="ENpc" />s offering the current shop.</returns>
        private ENpc[] BuildENpcs() {
            return Sheet.Collection.ENpcs.FindWithData(Key).ToArray();
        }

        /// <summary>
        ///     Build an array of the <see cref="GilShopItem" />s offered by the current shop.
        /// </summary>
        /// <returns>An array of the <see cref="GilShopItem" />s offered by the current shop.</returns>
        private GilShopItem[] BuildShopItems() {
            return Sheet.Collection.GetSheet2<GilShopItem>()
                .Cast<GilShopItem>()
                .Where(r => r.ParentKey == this.Key)
                .ToArray();
        }

        #endregion


        #region IItemSource Members

        private Item[] _ItemSourceItems;

        /// <summary>
        /// Gets the <see cref="Item"/>s that can be obtained from the current object.
        /// </summary>
        /// <value>The <see cref="Item"/>s that can be obtained from the current object.</value>
        IEnumerable<Item> IItemSource.Items {
            get { return _ItemSourceItems ?? (_ItemSourceItems = Items.Select(i => i.Item).ToArray()); }
        }

        #endregion

        #region IShop Members

        string IShop.Name {
            get { return Name; }
        }

        #endregion
    }
}
