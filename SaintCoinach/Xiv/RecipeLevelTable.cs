using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class RecipeLevelTable : XivRow {
        #region Properties

        public int Difficulty { get { return AsInt32("Difficulty"); } }
        public int Quality { get { return AsInt32("Quality"); } }
        public int Durability { get { return AsInt32("Durability"); } }

        #endregion

        #region Constructors

        #region Constructor

        public RecipeLevelTable(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
