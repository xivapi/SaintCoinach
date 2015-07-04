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
        ///     Gets the size factor of the current map.
        /// </summary>
        /// <remarks>
        ///     Base map size 41 units, the value of <c>SizeFactor</c> indicates by how much to divide this to get the size of the
        ///     current map.
        /// </remarks>
        /// <value>The size of the current map.</value>
        public int SizeFactor { get { return AsInt32("SizeFactor"); } }

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
                Image image;
                if (_MediumImage != null && _MediumImage.TryGetTarget(out image))
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
                Image image;
                if (_SmallImage != null && _SmallImage.TryGetTarget(out image))
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
            IO.File file;
            if (!pack.TryGetFile(filePath, out file))
                return null;

            var imageFile = new ImageFile(file.Pack, file.CommonHeader);

            var maskPath = string.Format(MapFileFormat, Id, fileName, "m", size);
            IO.File mask;
            if (pack.TryGetFile(maskPath, out mask))
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
                for (var j = 0; j < 3; ++j)     // Only blend RGB
                    result[i + j] = (byte)((aArgb[i + j] * bArgb[i + j]) / 255);
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

        /// <summary>
        ///     Returns a string representation of the map.
        /// </summary>
        /// <returns>The value of <see cref="Id" />.</returns>
        public override string ToString() {
            return Id;
        }
    }
}
