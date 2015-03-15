using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ENpcResident : XivRow, IQuantifiableName {
        #region Properties

        public string Singular { get { return AsString("Singular"); } }
        public string Plural { get { return AsString("Plural"); } }
        public string Title { get { return AsString("Title"); } }
        public Map Map { get { return As<Map>(); } }

        #endregion

        #region Constructors

        #region Constructor

        public ENpcResident(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Singular;
        }
    }
}
