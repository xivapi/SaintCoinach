using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.Items {
    public class Accessory : Armour {
        #region Constructors

        #region Constructor

        public Accessory(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
