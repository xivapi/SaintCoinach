using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class RelicNoteCategoryText : XivRow {
        #region Properties

        public Text.XivString Text { get { return AsString("Text"); } }

        #endregion

        #region Constructors

        #region Constructor

        public RelicNoteCategoryText(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Text;
        }
    }
}
