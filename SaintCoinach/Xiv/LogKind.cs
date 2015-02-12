using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class LogKind : XivRow {
        #region Properties

        public string Format { get { return AsString("Format"); } }
        public string Name { get { return AsString("Name"); } }
        public string Example { get { return AsString("Example"); } }
        public LogKindCategoryText LogKindCategoryText { get { return As<LogKindCategoryText>(); } }

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
