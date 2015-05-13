using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class FishParameter : XivRow {
        #region Properties

        public Text.XivString Text { get { return AsString("Text"); } }
        public Item Item { get { return As<Item>("Item"); } }

        #endregion

        #region Constructors

        #region Constructor

        public FishParameter(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return string.Format("{0}", Item);
        }
    }
}
