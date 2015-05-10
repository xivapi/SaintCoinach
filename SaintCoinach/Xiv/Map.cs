using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a map.
    /// </summary>
    public class Map : XivRow {
        #region Properties

        /// <summary>
        ///     Gets the identifier string of the current map.
        /// </summary>
        /// <value>The identifier string of the current map.</value>
        public string Id { get { return AsString("Id"); } }

        /// <summary>
        ///     Gets the size of the current map.
        /// </summary>
        /// <remarks>
        ///     Base map size 40 units, the value of <c>Size</c> indicates by how much to divide this to get the size of the
        ///     current map.
        /// </remarks>
        /// <value>The size of the current map.</value>
        public int Size { get { return AsInt32("Size"); } }

        /// <summary>
        ///     Gets the <see cref="PlaceName" /> of the region the current map is in.
        /// </summary>
        /// <value>The <see cref="PlaceName" /> of the region the current map is in.</value>
        public PlaceName RegionPlaceName { get { return As<PlaceName>("PlaceName{Region}"); } }

        /// <summary>
        ///     Gets the <see cref="PlaceName" /> of current map is in.
        /// </summary>
        /// <value>The <see cref="PlaceName" /> of current map is in.</value>
        public PlaceName PlaceName { get { return As<PlaceName>(); } }

        /// <summary>
        ///     Gets the <see cref="PlaceName" /> of the more specific location the current map is in.
        /// </summary>
        /// <value>The <see cref="PlaceName" /> of the more specific location the current map is in.</value>
        public PlaceName LocationPlaceName { get { return As<PlaceName>("PlaceName{Sub}"); } }

        /// <summary>
        ///     Gets the <see cref="TerritoryType" /> for the current map.
        /// </summary>
        /// <value>The <see cref="TerritoryType" /> for the current map.</value>
        public TerritoryType TerritoryType { get { return As<TerritoryType>(); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Map" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public Map(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Graphics
        public Graphics.Territory GetTerritory() {
            if (TerritoryType == null || TerritoryType.Key == 0)
                return null;

            var t = new Graphics.Territory(this.TerritoryType);
            if (t.Terrain == null && t.LgbFiles.Length == 0)
                return null;
            return t;
        }
        #endregion

        /// <summary>
        ///     Returns a string representation of the map.
        /// </summary>
        /// <returns>The value of <see cref="Id" />.</returns>
        public override string ToString() {
            return Id;
        }
    }
}
