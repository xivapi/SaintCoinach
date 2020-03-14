using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class LogKind : XivRow {
        #region Properties

        public Text.XivString Format { get { return AsString("Format"); } }
        public Text.XivString Name { get { return AsString("Name"); } }
        public Text.XivString Example { get { return AsString("Example"); } }

        #endregion

        #region Constructors

        #region Constructor

        public LogKind(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
