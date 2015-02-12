using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.Items {
    public class PhysicalWeapon : Weapon {
        #region Constructors

        #region Constructor

        public PhysicalWeapon(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
