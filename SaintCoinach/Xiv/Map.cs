using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a map.
    /// </summary>
    public class Map : XivRow {
        #region Fields
        private ImageFile _MediumImage;
        private ImageFile _SmallImage;
        #endregion

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

        /// <summary>
        ///     Gets the medium <see cref="ImageFile" /> of the current map.
        /// </summary>
        public ImageFile MediumImage
        {
            get { return _MediumImage ?? (_MediumImage = BuildImage("m")); }
        }

        /// <summary>
        ///     Gets the small <see cref="ImageFile" /> of the current map.
        /// </summary>
        public ImageFile SmallImage
        {
            get { return _SmallImage ?? (_SmallImage = BuildImage("s")); }
        }

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

        #region Build
        private ImageFile BuildImage(string size) {
            const string MapFileFormat = "ui/map/{0}/{1}_{2}.tex";

            if (string.IsNullOrEmpty(Id))
                return null;

            var fileName = Id.Replace("/", "");
            var filePath = string.Format(MapFileFormat, Id, fileName, size);

            IO.File file;
            if (Sheet.Collection.PackCollection.TryGetFile(filePath, out file))
                return new ImageFile(file.Pack, file.CommonHeader);

            return null;
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
