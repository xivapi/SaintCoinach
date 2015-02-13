using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing key items.
    /// </summary>
    public class EventItem : ItemBase {
        #region Properties

        /// <summary>
        ///     Gets the quest the current item is used for.
        /// </summary>
        /// <value>The quest the current item is used for.</value>
        public Quest Quest { get { return As<Quest>(); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EventItem" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public EventItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
