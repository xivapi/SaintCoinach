using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public abstract class HousingItem : XivRow {
        #region Properties

        public HousingItemCategory HousingItemCategory { get { return As<HousingItemCategory>(); } }
        public Item Item { get { return As<Item>("Item"); } }

        #endregion

        #region Constructors

        #region Constructor

        protected HousingItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return string.Format("{0}", Item);
        }
    }
}
