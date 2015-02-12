using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    // ReSharper disable once InconsistentNaming
    public class ItemUICategory : XivRow {
        #region Properties

        public string Name { get { return AsString("Name"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        public int MinorOrder { get { return AsInt32("Order{Minor}"); } }
        public int MajorOrder { get { return AsInt32("Order{Major}"); } }

        #endregion

        #region Constructors

        #region Constructor

        public ItemUICategory(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
