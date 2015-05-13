using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class GrandCompany : XivRow {
        #region Static

        private const int SealItemOffset = 19;

        #endregion

        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public Item SealItem { get { return Sheet.Collection.GetSheet<Item>()[Key + SealItemOffset]; } }

        #endregion

        #region Constructors

        #region Constructor

        public GrandCompany(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
