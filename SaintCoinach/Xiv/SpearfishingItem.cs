using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class SpearfishingItem : GatheringItemBase {
        #region Properties

        public int ItemLevel => AsInt32("ItemLevel");
        public bool IsVisible => AsBoolean("IsVisible");

        #endregion

        #region Constructors

        public SpearfishingItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

    }
}
