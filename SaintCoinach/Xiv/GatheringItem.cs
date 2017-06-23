using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class GatheringItem : GatheringItemBase {
        #region Properties

        public bool IsHidden => AsBoolean("IsHidden");

        #endregion

        #region Constructors

        public GatheringItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

    }
}
