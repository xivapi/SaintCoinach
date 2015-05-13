using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class LogKindCategoryText : XivRow {
        #region Properties

        public Text.XivString Text { get { return AsString("Text"); } }

        #endregion

        #region Constructors

        #region Constructor

        public LogKindCategoryText(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Text;
        }
    }
}
