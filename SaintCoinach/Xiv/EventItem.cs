using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class EventItem : Item {
        #region Properties

        public Quest Quest { get { return As<Quest>(); } }

        #endregion

        #region Constructors

        #region Constructor

        public EventItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
