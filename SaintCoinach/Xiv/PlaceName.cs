using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class PlaceName : XivRow {
        #region Properties

        public string Name { get { return AsString("Name"); } }
        public string NameWithoutArticle { get { return AsString("Name{NoArticle}"); } }

        #endregion

        #region Constructors

        #region Constructor

        public PlaceName(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
