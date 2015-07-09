using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a location in the game.
    /// </summary>
    public class Level : XivRow, ILocation {
        #region Properties

        /// <summary>
        ///     Gets the X-coordinate in world-space.
        /// </summary>
        /// <value>The X-coordinate in world-space.</value>
        public float X { get { return AsSingle("X"); } }

        /// <summary>
        ///     Gets the Y-coordinate in world-space.
        /// </summary>
        /// <value>The Y-coordinate in world-space.</value>
        public float Y { get { return AsSingle("Y"); } }

        /// <summary>
        ///     Gets the Z-coordinate in world-space.
        /// </summary>
        /// <value>The Z-coordinate in world-space.</value>
        public float Z { get { return AsSingle("Z"); } }

        /// <summary>
        ///     Gets the X-coordinate on the 2D-map.
        /// </summary>
        /// <value>The X-coordinate on the 2D-map.</value>
        public double MapX { get { return ToMapCoordinate(X); } }

        /// <summary>
        ///     Gets the Y-coordinate on the 2D-map.
        /// </summary>
        /// <value>The Y-coordinate on the 2D-map.</value>
        public double MapY { get { return ToMapCoordinate(Z); } }

        /// <summary>
        ///     Gets the yaw of the current location.
        /// </summary>
        /// <value>The yaw of the current location.</value>
        public float Yaw { get { return AsSingle("Yaw"); } }

        /// <summary>
        ///     Gets the radius of the current location.
        /// </summary>
        /// <value>The radius of the current location.</value>
        public float Radius { get { return AsSingle("Radius"); } }

        /// <summary>
        ///     Gets the type of the current location.
        /// </summary>
        /// <value>The type of the current location.</value>
        public int Type { get { return AsInt32("Type"); } }

        /// <summary>
        ///     Gets the key of the object associated witht he current location.
        /// </summary>
        /// <value>The key of the object associated witht he current location.</value>
        public int ObjectKey { get { return AsInt32("ObjectKey"); } }

        /// <summary>
        ///     Gets the <see cref="Map" /> the current location is in.
        /// </summary>
        /// <value>The <see cref="Map" /> the current location is in.</value>
        public Map Map { get { return As<Map>(); } }

        /// <summary>
        /// Gets the <see cref="Map"/> for the current object.
        /// </summary>
        /// <value>The <see cref="Map"/> for the current object.</value>
        PlaceName ILocation.PlaceName { get { return Map.PlaceName; } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Level" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public Level(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        /// <summary>
        ///     Convert a X- or Z-coordinate from world-space into its 2D-coordinate.
        /// </summary>
        /// <param name="val">The coordinate in world-space to convert.</param>
        /// <returns><c>val</c> converted into 2D-space.</returns>
        private double ToMapCoordinate(double val) {
            var c = Map.SizeFactor / 100.0;

            val *= c;
            return ((41.0 / c) * ((val + 1024.0) / 2048.0)) + 1;
        }
    }
}
