using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class GatheringItem : XivRow {
        #region Properties

        public Item Item { get { return As<Item>("Item"); } }
        public int ItemLevel { get { return AsInt32("ItemLevel"); } }
        public bool IsRare { get { return AsBoolean("IsRare"); } }

        #endregion

        #region Constructors

        #region Constructor

        public GatheringItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return string.Format("{0}", Item);
        }
    }
}
