using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class CompanyAction : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public Text.XivString Description { get { return AsString("Description"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        // ReSharper disable once InconsistentNaming
        public FCRank FCRank { get { return As<FCRank>(); } }
        public int Cost { get { return AsInt32("Cost"); } }
        public int Order { get { return AsInt32("Order"); } }

        #endregion

        #region Constructors

        #region Constructor

        public CompanyAction(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
