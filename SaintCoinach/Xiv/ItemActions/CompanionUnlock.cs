using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.ItemActions {
    public class CompanionUnlock : ItemAction {
        #region Static

        private const int CompanionKey = 0;

        #endregion

        #region Properties

        public Companion Companion {
            get {
                var key = GetData(CompanionKey);
                return Sheet.Collection.GetSheet<Companion>()[key];
            }
        }

        #endregion

        #region Constructors

        #region Constructor

        public CompanionUnlock(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
