using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class LogFilter : XivRow {
        #region Properties

        public string Name { get { return AsString("Name"); } }
        public LogKind LogKind { get { return As<LogKind>(); } }
        public string Example { get { return AsString("Example"); } }

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
