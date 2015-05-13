using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     Class representing the category for items in the UI.
    /// </summary>
    /// <remarks>
    ///     This class is used when displaying an item's category, as well as inventory sorting.
    ///     When sorting an inventory with the Standard method it groups by <see cref="MajorOrder" /> first, sorts the groups
    ///     in ascending order by key, then does the same for <see cref="MinorOrder" /> inside each group. The items inside
    ///     minor groups are sorted according to additional rules.
    /// </remarks>
    public class ItemUICategory : XivRow {
        #region Properties

        /// <summary>
        ///     Gets the name of the current category.
        /// </summary>
        /// <value>The name of the current category.</value>
        public Text.XivString Name { get { return AsString("Name"); } }

        /// <summary>
        ///     Gets the icon for the current category.
        /// </summary>
        /// <value>The icon of the current category.</value>
        public ImageFile Icon { get { return AsImage("Icon"); } }

        /// <summary>
        ///     Gets the minor order of the current category.
        /// </summary>
        /// <value>The minor order of the current category.</value>
        public int MinorOrder { get { return AsInt32("Order{Minor}"); } }

        /// <summary>
        ///     Gets the major order of the current category.
        /// </summary>
        /// <value>The major order of the current category.</value>
        public int MajorOrder { get { return AsInt32("Order{Major}"); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ItemUICategory" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public ItemUICategory(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        /// <summary>
        ///     Returns a string representation of the current category.
        /// </summary>
        /// <returns>The value of <see cref="Name" />.</returns>
        public override string ToString() {
            return Name;
        }
    }
}
