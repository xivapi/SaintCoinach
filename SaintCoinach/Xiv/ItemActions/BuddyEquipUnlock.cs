using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.ItemActions {
    public class BuddyEquipUnlock : ItemAction {
        #region Static

        private const int BuddyEquipIndex = 0;

        #endregion

        #region Properties

        public BuddyEquip BuddyEquip {
            get {
                var key = GetData(BuddyEquipIndex);
                return Sheet.Collection.GetSheet<BuddyEquip>()[key];
            }
        }

        #endregion

        #region Constructors

        #region Constructor

        public BuddyEquipUnlock(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
