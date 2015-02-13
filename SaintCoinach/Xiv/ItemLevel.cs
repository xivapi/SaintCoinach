using System;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing an item level.
    /// </summary>
    public class ItemLevel : XivRow {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ItemLevel" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public ItemLevel(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Helpers

        /// <summary>
        ///     Get the maximum for a <see cref="BaseParam" /> at the current item level.
        /// </summary>
        /// <remarks>
        ///     The value returned by this method is just the base on which further calculations are performed.
        /// </remarks>
        /// <param name="baseParam">The <see cref="BaseParam" /> for which to get the maximum.</param>
        /// <returns>The maximum value of <c>baseParam</c> at the current item level.</returns>
        public int GetMaximum(BaseParam baseParam) {
            const int Offset = -1;
            return baseParam.Key == 0 ? 0 : Convert.ToInt32(this[Offset + baseParam.Key]);
        }

        #endregion

        /// <summary>
        ///     Returns a string representation of the current item level.
        /// </summary>
        /// <returns>The string representation of the current item level.</returns>
        public override string ToString() {
            return Key.ToString();
        }
    }
}
