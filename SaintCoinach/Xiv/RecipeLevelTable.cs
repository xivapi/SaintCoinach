using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing the table used as base for difficulty, quality and durability operations on
    ///     <see cref="Recipe" />s.
    /// </summary>
    public class RecipeLevelTable : XivRow {
        #region Properties

        public int CharacterLevel { get { return AsInt32("CharacterLevel"); } }

        /// <summary>
        ///     Gets the base difficulty for the current level.
        /// </summary>
        /// <value>The base difficulty for the current level.</value>
        public int Difficulty { get { return AsInt32("Difficulty"); } }

        /// <summary>
        ///     Gets the base quality for the current level.
        /// </summary>
        /// <value>The base quality for the current level.</value>
        public int Quality { get { return AsInt32("Quality"); } }

        /// <summary>
        ///     Gets the base durability for the current level.
        /// </summary>
        /// <value>The base durability for the current level.</value>
        public int Durability { get { return AsInt32("Durability"); } }

        /// <summary>
        ///     Gets the base star difficulty for the current level.
        /// </summary>
        /// <value>The base star difficulty for the current level.</value>
        public int Stars { get { return AsInt32("Stars"); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecipeLevelTable" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public RecipeLevelTable(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
