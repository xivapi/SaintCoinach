using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public abstract class HousingItem : XivRow {
        #region Properties

        public int ModelKey { get { return AsInt32("ModelKey"); } }
        public HousingItemCategory HousingItemCategory { get { return As<HousingItemCategory>(); } }
        public Item Item { get { return As<Item>("Item"); } }
        public HousingLayoutLimit HousingLayoutLimit { get { return As<HousingLayoutLimit>(); } }
        public bool DestroyedOnRemoval { get { return AsBoolean("DestroyOnRemoval"); } }

        #endregion

        #region Constructors

        protected HousingItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public abstract Graphics.Sgb.SgbFile GetScene();

        public override string ToString() {
            return string.Format("{0}", Item);
        }
    }
}
