using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class HousingFurniture : HousingItem {
        #region Constructors

        #region Constructor

        public HousingFurniture(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
