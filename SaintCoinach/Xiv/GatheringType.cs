using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class GatheringType : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public ImageFile MainIcon { get { return AsImage("IconMain"); } }
        public ImageFile SubIcon { get { return AsImage("IconOff"); } }

        #endregion

        #region Constructors

        public GatheringType(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
