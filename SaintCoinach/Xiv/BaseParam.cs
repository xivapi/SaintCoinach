using System;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Class representing parameters from the game data.
    /// </summary>
    public class BaseParam : XivRow {
        #region Properties
        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public Text.XivString Name { get { return AsString("Name"); } }
        /// <summary>
        /// Gets the description of the parameter.
        /// </summary>
        /// <remarks>Not all parameters have a description.</remarks>
        /// <value>The description of the parameter.</value>
        public Text.XivString Description { get { return AsString("Description"); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParam"/> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet"/> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow"/> to read data from.</param>
        public BaseParam(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion


        /// <summary>
        ///     Returns a string that represents the current <see cref="BaseParam" />.
        /// </summary>
        /// <returns>Returns the value of <see cref="Name" />.</returns>
        public override string ToString() {
            return Name;
        }

        #region Helpers

        /// <summary>
        /// Get the maximum value of the current <see cref="BaseParam"/> for a specific <see cref="EquipSlotCategory"/>.
        /// </summary>
        /// <param name="category"><see cref="EquipSlotCategory"/> to get the maximum parameter value for.</param>
        /// <returns>Returns the maximum value for the current <see cref="BaseParam"/> on <c>category</c>.</returns>
        public int GetMaximum(EquipSlotCategory category) {
            const int Offset = 2;
            return category.Key == 0 ? 0 : Convert.ToInt32(this[Offset + category.Key]);
        }

        /// <summary>
        /// Get the value modifier of the current <see cref="BaseParam"/> for a certain role.
        /// </summary>
        /// <param name="role">Role to get the modifier for.</param>
        /// <returns>Returns the modifier for <c>role</c>, in percent.</returns>
        public int GetModifier(int role) {
            const int Offset = 24;
            const int Maximum = 12;
            if (role < 0 || role > Maximum)
                return 0;
            return Convert.ToInt32(this[Offset + role]);
        }

        #endregion
    }
}
