using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class Title : XivRow {
        #region Properties

        public Text.XivString Masculine { get { return AsString("Masculine"); } }
        public Text.XivString Feminine { get { return AsString("Feminine"); } }
        public bool IsPrefix { get { return AsBoolean("IsPrefix"); } }

        #endregion

        #region Constructors

        #region Constructor

        public Title(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return string.Format("{0} / {1}", Feminine, Masculine);
        }
    }
}
