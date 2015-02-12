using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.Items {
    public class Housing : InventoryItem {
        #region Constructors

        #region Constructor

        public Housing(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
