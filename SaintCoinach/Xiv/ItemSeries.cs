using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ItemSeries : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        #endregion

        #region Constructors

        #region Constructor

        public ItemSeries(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
