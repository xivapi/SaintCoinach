using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class BNpcName : XivRow {
        #region Properties

        public string Singular { get { return AsString("Singular"); } }
        public string Plural { get { return AsString("Plural"); } }

        #endregion

        #region Constructors

        #region Constructor

        public BNpcName(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Singular;
        }
    }
}
