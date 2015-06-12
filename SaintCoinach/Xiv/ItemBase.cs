using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Base class for items from the game data.
    /// </summary>
    /// <remarks>
    /// Inherited by <see cref="EventItem"/> and <see cref="Item"/>.
    /// </remarks>
    public abstract class ItemBase : XivRow, IQuantifiableXivString {
        #region Properties
        /// <summary>
        /// Gets the name of the current item.
        /// </summary>
        /// <value>The name of the current item.</value>
        public Text.XivString Name { get { return AsString("Name"); } }
        /// <summary>
        /// Gets the description of the current item.
        /// </summary>
        /// <value>The description of the current item.</value>
        public Text.XivString Description { get { return AsString("Description"); } }
        /// <summary>
        /// Gets the singular string of the current item.
        /// </summary>
        /// <value>The singular string of the current item.</value>
        public Text.XivString Singular { get { return AsString("Singular"); } }
        /// <summary>
        /// Gets the plural string of the current item.
        /// </summary>
        /// <value>The plural string of the current item.</value>
        public Text.XivString Plural { get { return Sheet.Collection.ActiveLanguage == Ex.Language.Japanese ? Singular : AsString("Plural"); } }
        /// <summary>
        /// Gets the icon of the current item.
        /// </summary>
        /// <value>The icon of the current item.</value>
        public ImageFile Icon { get { return AsImage("Icon"); } }
        /// <summary>
        /// Gets the stack size of the current item.
        /// </summary>
        /// <value>The stack size of the current item.</value>
        public int StackSize { get { return AsInt32("StackSize"); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ItemBase" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        protected ItemBase(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        /// <summary>
        ///     Returns a string that represents the current <see cref="ItemBase" />.
        /// </summary>
        /// <returns>Returns the value of <see cref="Name" />.</returns>
        public override string ToString() {
            return Name;
        }

        #region IQuantifiableName Members
        string IQuantifiable.Singular {
            get { return Singular; }
        }

        string IQuantifiable.Plural {
            get { return Plural; }
        }
        #endregion
    }
}
