using System;
using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a fishing spot from the game data.
    /// </summary>
    public class FishingSpot : XivRow, IItemSource, ILocatable, ILocation {
        #region Fields

        /// <summary>
        ///     Items that can be fished up at the current <see cref="FishingSpot" />.
        /// </summary>
        private Item[] _Items;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the level of the current <see cref="FishingSpot" />.
        /// </summary>
        /// <value>The level of the current <see cref="FishingSpot" />.</value>
        public int Level { get { return AsInt32("Level"); } }

        /// <summary>
        ///     Gets the text indicating special conditions have been met.
        /// </summary>
        /// <value>The text indicating special conditions have been met.</value>
        public Text.XivString OnReachBigFish { get { return AsString("BigFish{OnReach}"); } }

        /// <summary>
        ///     Gets the text indicating special conditions have ended.
        /// </summary>
        /// <value>The text indicating special conditions have ended.</value>
        public Text.XivString OnEndBigFish { get { return AsString("BigFish{OnEnd}"); } }

        /// <summary>
        ///     Gets the <see cref="FishingSpotCategory" /> of the current <see cref="FishingSpot" />.
        /// </summary>
        /// <value>The <see cref="FishingSpotCategory" /> of the current <see cref="FishingSpot" />.</value>
        public FishingSpotCategory FishingSpotCategory { get { return As<FishingSpotCategory>(); } }

        /// <summary>
        ///     Gets the <see cref="TerritoryType" /> the current <see cref="FishingSpot" /> is in.
        /// </summary>
        /// <value>The <see cref="TerritoryType" /> the current <see cref="FishingSpot" /> is in.</value>
        public TerritoryType TerritoryType { get { return As<TerritoryType>(); } }

        /// <summary>
        ///     Gets the X-coordinate of the fishing spot in the <see cref="TerritoryType" />.
        /// </summary>
        /// <remarks>
        ///     This coordinate is in 3D-space.
        /// </remarks>
        /// <value>The X-coordinate of the fishing spot in the <see cref="TerritoryType" />.</value>
        public int X { get { return AsInt32("X"); } }

        /// <summary>
        ///     Gets the Z-coordinate of the fishing spot in the <see cref="TerritoryType" />.
        /// </summary>
        /// <remarks>
        ///     This coordinate is in 3D-space.
        /// </remarks>
        /// <value>The Z-coordinate of the fishing spot in the <see cref="TerritoryType" />.</value>
        public int Z { get { return AsInt32("Z"); } }

        /// <summary>
        ///     Gets the radius of the current <see cref="FishingSpot" />.
        /// </summary>
        /// <remarks>
        ///     Value is most likely in map-space, and only used when displaying the map of fishing spot.
        /// </remarks>
        /// <value>The radius of the current <see cref="FishingSpot" />.</value>
        public int Radius { get { return AsInt32("Radius"); } }

        /// <summary>
        ///     Gets the <see cref="PlaceName" /> of the current <see cref="FishingSpot" />.
        /// </summary>
        /// <value>The <see cref="PlaceName" /> of the current <see cref="FishingSpot" />.</value>
        public PlaceName PlaceName { get { return As<PlaceName>(); } }

        /// <summary>
        ///     Gets the items that can be obtained at the current <see cref="FishingSpot" />.
        /// </summary>
        /// <value>The items that can be obtained at the current <see cref="FishingSpot" />.</value>
        public IEnumerable<Item> Items { get { return _Items ?? (_Items = BuildItems()); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FishingSpot" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public FishingSpot(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build

        /// <summary>
        ///     Create an array of items that can be obtained at the current <see cref="FishingSpot" />.
        /// </summary>
        /// <returns>Returns an array of items that can be obtained at the current <see cref="FishingSpot" />.</returns>
        private Item[] BuildItems() {
            const int Count = 10;

            var items = new List<Item>();
            for (var i = 0; i < Count; ++i) {
                var item = As<Item>("Item", i);
                if (item.Key != 0)
                    items.Add(item);
            }

            return items.ToArray();
        }

        #endregion

        #region Location
        public double MapX { get { return ToMapCoordinate(X); } }

        public double MapY { get { return ToMapCoordinate(Z); } }

        IEnumerable<ILocation> ILocatable.Locations {
            get {
                yield return this;
            }
        }

        private double ToMapCoordinate(double val) {
            var c = TerritoryType.Map.SizeFactor / 100.0;

            return (41.0 / c) * ((val) / 2048.0) + 1;
        }
        #endregion

        /// <summary>
        ///     Returns a string that represents the current <see cref="FishingSpot" />.
        /// </summary>
        /// <returns>Returns the value of <see cref="PlaceName" />.</returns>
        public override string ToString() {
            return string.Format("{0}", PlaceName);
        }
    }
}
