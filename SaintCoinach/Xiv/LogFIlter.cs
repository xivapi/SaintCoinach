using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class LogFilter : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public LogKind LogKind { get { return As<LogKind>(); } }
        public Text.XivString Example { get { return AsString("Example"); } }

        #endregion

        #region Constructors

        #region Constructor

        public LogFilter(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
