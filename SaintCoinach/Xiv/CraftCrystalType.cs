using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class CraftCrystalType : XivRow {
        #region Properties

        public Item Item { get { return As<Item>("Item"); } }

        #endregion

        #region Constructors

        #region Constructor

        public CraftCrystalType(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
