using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing actions invoked when items are used.
    /// </summary>
    public class ItemAction : XivRow {
        #region Static

        /// <summary>
        ///     The number of data fields present.
        /// </summary>
        /// <remarks>
        ///     This value indicates how many fields are for NQ and HQ data each.
        /// </remarks>
        public const int DataCount = 8;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the type of the current item action.
        /// </summary>
        /// <value>The type of the current item action.</value>
        public int Type { get { return AsInt32("Type"); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ItemAction" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public ItemAction(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Helpers

        /// <summary>
        ///     Get the integer value at a specified data field for NQ items.
        /// </summary>
        /// <param name="index">Index of the data field to get.</param>
        /// <returns>The integer value at a specified data field for NQ items.</returns>
        public int GetData(int index) {
            return AsInt32("Data", index);
        }

        /// <summary>
        ///     Get the integer value at a specified data field for HQ items.
        /// </summary>
        /// <param name="index">Index of the data field to get.</param>
        /// <returns>The integer value at a specified data field for HQ items.</returns>
        public int GetHqData(int index) {
            return AsInt32("Data{HQ}", index);
        }

        #endregion
    }
}
