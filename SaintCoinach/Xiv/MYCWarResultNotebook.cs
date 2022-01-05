using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    // This is for bozja field notes
    public class MYCWarResultNotebook : XivRow {
        #region Properties
        public Text.XivString Name { get { return AsString("Name"); } }
        public Text.XivString Description { get { return AsString("Description"); } }

        public ImageFile Icon { get { return AsImage("Icon"); } }
        public ImageFile Image { get { return AsImage("Image"); } }

        public short Rarity { get { return AsInt16("Rarity"); } }
        #endregion

        #region Constructor
        public MYCWarResultNotebook(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}
