using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class RelicNoteCategory : XivRow {
        #region Properties

        public Text.XivString Text { get { return AsString("Text"); } }
        public RelicNoteCategoryText RelicNoteCategoryText { get { return As<RelicNoteCategoryText>(); } }

        #endregion

        #region Constructors

        #region Constructor

        public RelicNoteCategory(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Text;
        }
    }
}
