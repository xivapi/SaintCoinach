using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public abstract class HousingItem : XivRow {
        #region Properties

        public int ModelKey => AsInt32("ModelKey");
        public HousingItemCategory HousingItemCategory => As<HousingItemCategory>();
        public Item Item => As<Item>("Item");
        public HousingLayoutLimit HousingLayoutLimit => As<HousingLayoutLimit>();
        public bool DestroyedOnRemoval => AsBoolean("DestroyOnRemoval");

        #endregion

        #region Constructors

        protected HousingItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public abstract Graphics.Sgb.SgbFile GetScene();

        public override string ToString() => string.Format("{0}", Item);
    }
}
