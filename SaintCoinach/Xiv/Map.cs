using System;
using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;
using System.Drawing;
using System.Drawing.Imaging;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a map.
    /// </summary>
    public class Map : XivRow {
        #region Fields
        private WeakReference<Image> _MediumImage;
        private WeakReference<Image> _SmallImage;
        #endregion

        #region Properties

        public sbyte Index => As<sbyte>("MapIndex");

        /// <summary>
        ///     Gets the identifier string of the current map.
        /// </summary>
        /// <value>The identifier string of the current map.</value>
        public Text.XivString Id { get { return AsString("Id"); } }

        /// <summary>
        ///     Gets the hierarchy level of the current map.
        /// </summary>
        /// <value>The hierarchy level of the current map.</value>
        public int Hierarchy { get { return AsInt32("Hierarchy"); } }

        /// <summary>
        ///     Gets the MapMarker parent key range of the current map.
        /// </summary>
        /// <value>The MapMarker parent key range of the current map.</value>
        public int MapMarkerRange {  get { return AsInt32("MapMarkerRange"); } }

        /// <summary>
        ///     Gets the size factor of the current map.
        /// </summary>
        /// <remarks>
        ///     Base map size 41 units, the value of <c>SizeFactor</c> indicates by how much to divide this to get the size of the
        ///     current map.
        /// </remarks>
        /// <value>The size of the current map.</value>
        public int SizeFactor { get { return AsInt32("SizeFactor"); } }

        /// <summary>
        ///     Gets the X value offset of the current map.
        /// </summary>
        public int OffsetX { get { return AsInt32("Offset{X}"); } }

        /// <summary>
        ///     Gets the Y value offset of the current map.
        /// </summary>
        public int OffsetY { get { return AsInt32("Offset{Y}"); } }

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
        ///     Gets the masked medium <see cref="Image" /> of the current map.
        /// </summary>
        public Image MediumImage {
            get {
                if (_MediumImage != null && _MediumImage.TryGetTarget(out var image))
                    return image;
                image = BuildImage("m");
                if (_MediumImage == null)
                    _MediumImage = new WeakReference<Image>(image);
                else
                    _MediumImage.SetTarget(image);
                return image;
            }
        }

        /// <summary>
        ///     Gets the masked small <see cref="Image" /> of the current map.
        /// </summary>
        public Image SmallImage {
            get {
                if (_SmallImage != null && _SmallImage.TryGetTarget(out var image))
                    return image;
                image = BuildImage("s");
                if (_SmallImage == null)
                    _SmallImage = new WeakReference<Image>(image);
                else
                    _SmallImage.SetTarget(image);
                return image;
            }
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
        private Image BuildImage(string size) {
            const string MapFileFormat = "ui/map/{0}/{1}{2}_{3}.tex";

            if (string.IsNullOrEmpty(Id))
                return null;

            var fileName = Id.ToString().Replace("/", "");
            var pack = Sheet.Collection.PackCollection;

            var filePath = string.Format(MapFileFormat, Id, fileName, string.Empty, size);
            if (!pack.TryGetFile(filePath, out var file))
                return null;

            var imageFile = new ImageFile(file.Pack, file.CommonHeader);

            var maskPath = string.Format(MapFileFormat, Id, fileName, "m", size);
            if (pack.TryGetFile(maskPath, out var mask))
            {
                // Multiply the mask against the file.
                var maskFile = new ImageFile(mask.Pack, mask.CommonHeader);
                return MultiplyBlend(imageFile, maskFile);
            }

            return imageFile.GetImage();
        }

        private static Image MultiplyBlend(ImageFile image, ImageFile mask) {
            if (image.Width != mask.Width || image.Height != mask.Height)
                throw new ArgumentException();
            // Using 32bit color
            const int BytesPerPixel = 4;

            var aArgb = Imaging.ImageConverter.GetA8R8G8B8(image);
            var bArgb = Imaging.ImageConverter.GetA8R8G8B8(mask);
            var result = new byte[aArgb.Length];

            for (var i = 0; i < aArgb.Length; i += BytesPerPixel) {
                // There are other algorithms that can do this with any alpha,
                // but I haven't the time to research them now.
                var bAlpha = bArgb[i + 3];
                if (bAlpha == 0) {
                    // Mask pixel is transparent, do not blend.
                    result[i] = aArgb[i];
                    result[i + 1] = aArgb[i + 1];
                    result[i + 2] = aArgb[i + 2];
                } else { 
                    for (var j = 0; j < 3; ++j)     // Only blend RGB
                        result[i + j] = (byte)((aArgb[i + j] * bArgb[i + j]) / 255);
                }
                result[i + 3] = aArgb[i + 3];  // Preserve alpha
            }

            Image output;
            unsafe {
                fixed (byte* p = result) {
                    var ptr = (IntPtr)p;
                    using (var tempImage = new Bitmap(image.Width, image.Height, image.Width * BytesPerPixel, PixelFormat.Format32bppArgb, ptr))
                        output = new Bitmap(tempImage);
                }
            }
            return output;
        }
        #endregion

        #region Coordinates
        /// <summary>
        ///     Convert a X- or Y-coordinate into an offset, map-scaled 2D-coordinate.
        /// </summary>
        /// <param name="value">The coordinate to convert.</param>
        /// <param name="offset">The coordinate offset from this map.</param>
        /// <returns><c>value</c> converted and scaled to this map.</returns>
        public double ToMapCoordinate2d(int value, int offset)
        {
            var c = SizeFactor / 100.0;
            var offsetValue = value + offset;
            return (41.0 / c) * (offsetValue / 2048.0) + 1;
        }

        /// <summary>
        ///     Convert a X- or Z-coordinate from world-space into its 2D-coordinate.
        /// </summary>
        /// <param name="value">The coordinate in world-space to convert.</param>
        /// <param name="offset">The coordinate offset from this map in world-space.</param>
        /// <returns><c>value</c> converted into 2D-space.</returns>
        public double ToMapCoordinate3d(double value, int offset)
        {
            var c = SizeFactor / 100.0;
            var offsetValue = (value + offset) * c;
            return ((41.0 / c) * ((offsetValue + 1024.0) / 2048.0)) + 1;
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
