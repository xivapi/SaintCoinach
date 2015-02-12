using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class LeveClient : XivRow {
        #region Properties

        public string Name { get { return AsString("Name"); } }

        #endregion

        #region Constructors

        #region Constructor

        public LeveClient(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
