using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public abstract class GatheringItemBase : XivRow {
        #region Properties

        public ItemBase Item => As<ItemBase>("Item");

        #endregion

        #region Constructors

        public GatheringItemBase(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return string.Format("{0}", Item);
        }
    }
}
