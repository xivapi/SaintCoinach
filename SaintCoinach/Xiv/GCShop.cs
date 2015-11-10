using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     Class representing a Grand Company shop from the game data.
    /// </summary>
    public class GCShop : XivRow, IShop, ILocatable, IItemSource {
        #region Fields

        /// <summary>
        ///     <see cref="ENpcs" /> offering the current <see cref="GCShop" />.
        /// </summary>
        private ENpc[] _ENpcs;

        /// <summary>
        ///     Items offered by the current <see cref="GCShop" />.
        /// </summary>
        private GrandCompanySealShopItem[] _Items;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the key of the first <see cref="GrandCompanySealShopItem" /> in the current <see cref="GCShop" />.
        /// </summary>
        /// <value>The key of the first <see cref="GrandCompanySealShopItem" /> in the current <see cref="GCShop" />.</value>
        public int Min { get { return AsInt32("Min"); } }

        /// <summary>
        ///     Gets the key of the last <see cref="GrandCompanySealShopItem" /> in the current <see cref="GCShop" />.
        /// </summary>
        /// <value>The key of the last <see cref="GrandCompanySealShopItem" /> in the current <see cref="GCShop" />.</value>
        public int Max { get { return AsInt32("Max"); } }

        /// <summary>
        ///     Gets the <see cref="GrandCompany" /> of the current <see cref="GCShop" />.
        /// </summary>
        /// <value>The <see cref="GrandCompany" /> of the current <see cref="GCShop" />.</value>
        public GrandCompany GrandCompany { get { return As<GrandCompany>(); } }

        /// <summary>
        ///     Gets the items offered by the current <see cref="GCShop" />.
        /// </summary>
        /// <value>The items offered by the current <see cref="GCShop" />.</value>
        public IEnumerable<GrandCompanySealShopItem> Items { get { return _Items ?? (_Items = BuildItems()); } }

        /// <summary>
        /// Gets the locations of the current object.
        /// </summary>
        /// <value>The locations of the current object.</value>
        public IEnumerable<ILocation> Locations { get { return ENpcs.SelectMany(_ => _.Locations); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GCShop" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public GCShop(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        /// <summary>
        ///     Gets the <see cref="ENpcs" /> offering the current shop.
        /// </summary>
        /// <value>The <see cref="ENpcs" /> offering the current shop.</value>
        public IEnumerable<ENpc> ENpcs { get { return _ENpcs ?? (_ENpcs = BuildENpcs()); } }

        /// <summary>
        ///     Gets the listings of the current shop.
        /// </summary>
        /// <value>The listings of the current shop.</value>
        IEnumerable<IShopListing> IShop.ShopListings { get { return Items; } }

        /// <summary>
        ///     Gets the name of the current shop.
        /// </summary>
        /// <value>The name of the current shop's <see cref="GrandCompany" />.</value>
        string IShop.Name { get { return string.Format("{0}", GrandCompany); } }

        #region Build

        /// <summary>
        ///     Build the array of <see cref="ENpc" />s offering this shop.
        /// </summary>
        /// <returns>An array of <see cref="ENpc" />s offering this shop.</returns>
        private ENpc[] BuildENpcs() {
            return Sheet.Collection.ENpcs.FindWithData(Key).ToArray();
        }

        /// <summary>
        ///     Build the array of <see cref="GrandCompanySealShopItem" />s offered by this shop.
        /// </summary>
        /// <remarks>
        ///     This method takes all <see cref="GrandCompanySealShopItem" />s with keys from <see cref="Min" /> to
        ///     <see cref="Max" /> (inclusive).
        /// </remarks>
        /// <returns>An array of <see cref="GrandCompanySealShopItem" />s offered by this shop.</returns>
        private GrandCompanySealShopItem[] BuildItems() {
            var items = new List<GrandCompanySealShopItem>();

            var gcItems = Sheet.Collection.GetSheet<GrandCompanySealShopItem>();
            for (var i = Min; i <= Max; ++i) {
                var item = gcItems[i];
                if (item.Item.Key != 0)
                    items.Add(item);
            }

            return items.ToArray();
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

        public override string ToString() {
            return GrandCompany.ToString();
        }
    }
}
