using System.Drawing;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a dye.
    /// </summary>
    public class Stain : XivRow {
        #region Properties

        /// <summary>
        ///     Gets the name of the current stain.
        /// </summary>
        /// <value>The name of the current stain.</value>
        public Text.XivString Name { get { return AsString("Name"); } }

        /// <summary>
        ///     Gets the <see cref="Item" /> used for the current stain.
        /// </summary>
        /// <value>The <see cref="Item" /> used for the current stain.</value>
        public Item Item { get { return As<Item>("Item"); } }

        /// <summary>
        ///     Gets the key of the shade for the current stain.
        /// </summary>
        /// <value>The key of the shade for the current stain.</value>
        public int Shade { get { return AsInt32("Shade"); } }

        /// <summary>
        ///     Gets the colour of the current stain.
        /// </summary>
        /// <value>The colour of the current stain.</value>
        public Color Color { get { return As<Color>(); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Stain" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public Stain(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        /// <summary>
        ///     Returns a string representation of the current shop.
        /// </summary>
        /// <returns>The value of <see cref="Name" />.</returns>
        public override string ToString() {
            return Name;
        }
    }
}
