using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ENpcResident : XivRow, IQuantifiableXivString {
        #region Properties

        public Text.XivString Singular { get { return AsString("Singular"); } }
        public Text.XivString Plural { get { return Sheet.Collection.ActiveLanguage == Ex.Language.Japanese ? Singular : AsString("Plural"); } }
        public Text.XivString Title { get { return AsString("Title"); } }
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

        #region IQuantifiableName Members
        string IQuantifiable.Singular {
            get { return Singular; }
        }

        string IQuantifiable.Plural {
            get { return Plural; }
        }
        #endregion
    }
}
