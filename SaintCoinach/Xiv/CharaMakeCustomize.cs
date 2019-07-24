using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class CharaMakeCustomize : XivRow, IXivRow {
        #region Properties
        public int FeatureID { get { return AsInt32("FeatureID"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        public int Data { get { return AsInt32("Data"); } }
        public bool IsPurchasable => AsBoolean("IsPurchasable");
        #endregion

        public CharaMakeCustomize(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

    }
}