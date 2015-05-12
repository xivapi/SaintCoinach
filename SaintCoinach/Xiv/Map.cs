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
        private Image _MediumImage;
        private Image _SmallImage;
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
        ///     Gets the masked medium <see cref="Image" /> of the current map.
        /// </summary>
        public Image MediumImage
        {
            get { return _MediumImage ?? (_MediumImage = BuildImage("m")); }
        }

        /// <summary>
        ///     Gets the masked small <see cref="Image" /> of the current map.
        /// </summary>
        public Image SmallImage
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
        private Image BuildImage(string size) {
            const string MapFileFormat = "ui/map/{0}/{1}{2}_{3}.tex";

            if (string.IsNullOrEmpty(Id))
                return null;

            var fileName = Id.Replace("/", "");
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

        private static Image MultiplyBlend(ImageFile image, ImageFile mask)
        {
            // Some assumptions made about the number of bytes per pixel here.
            const int BytesPerPixel = 4;

            var aSrc = (Bitmap)image.GetImage();
            var bSrc = (Bitmap)mask.GetImage();
            var result = new Bitmap(aSrc.Width, aSrc.Height, aSrc.PixelFormat);
            result.MakeTransparent();

            var rect = new Rectangle(0, 0, aSrc.Width, aSrc.Height);
            var aData = aSrc.LockBits(rect, ImageLockMode.ReadOnly, aSrc.PixelFormat);
            var bData = bSrc.LockBits(rect, ImageLockMode.ReadOnly, bSrc.PixelFormat);
            var rData = result.LockBits(rect, ImageLockMode.WriteOnly, result.PixelFormat);

            unsafe
            {
                for (var y = 0; y < aSrc.Height; y++)
                {
                    var a = (byte*)aData.Scan0 + y * aData.Stride;
                    var b = (byte*)bData.Scan0 + y * bData.Stride;
                    var r = (byte*)rData.Scan0 + y * rData.Stride;

                    for (var x = 0; x < bSrc.Width * BytesPerPixel; x += BytesPerPixel)
                    {
                        r[x] = (byte)((a[x] * b[x]) / 255); // Blue
                        r[x + 1] = (byte)((a[x + 1] * b[x + 1]) / 255); // Green
                        r[x + 2] = (byte)((a[x + 2] * b[x + 2]) / 255); // Red
                        r[x + 3] = a[x + 3]; // Preserve alpha
                    }
                }
            }

            aSrc.UnlockBits(aData);
            bSrc.UnlockBits(bData);
            result.UnlockBits(rData);

            return result;
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
