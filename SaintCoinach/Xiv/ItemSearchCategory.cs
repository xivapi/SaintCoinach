using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class ItemSearchCategory : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        public int Category { get { return AsInt32("Category"); } }
        public int Order { get { return AsInt32("Order"); } }
        public ClassJob ClassJob { get { return As<ClassJob>(); } }

        #endregion

        #region Constructors

        #region Constructor

        public ItemSearchCategory(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
