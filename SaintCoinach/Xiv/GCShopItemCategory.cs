using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    // ReSharper disable once InconsistentNaming
    public class GCShopItemCategory : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        #endregion

        #region Constructors

        #region Constructor

        public GCShopItemCategory(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
