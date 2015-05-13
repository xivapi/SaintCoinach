using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class LogMessage : XivRow {
        #region Properties

        public Text.XivString Text { get { return AsString("Text"); } }
        public LogKind LogKind { get { return As<LogKind>(); } }

        #endregion

        #region Constructors

        #region Constructor

        public LogMessage(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Text;
        }
    }
}
