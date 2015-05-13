using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class RecipeElement : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        #endregion

        #region Constructors

        #region Constructor

        public RecipeElement(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
