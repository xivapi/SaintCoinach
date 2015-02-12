using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.ItemActions {
    public class MountUnlock : ItemAction {
        #region Static

        private const int MountKey = 0;

        #endregion

        #region Properties

        public Mount Mount {
            get {
                var key = GetData(MountKey);
                return Sheet.Collection.GetSheet<Mount>()[key];
            }
        }

        #endregion

        #region Constructors

        #region Constructor

        public MountUnlock(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
