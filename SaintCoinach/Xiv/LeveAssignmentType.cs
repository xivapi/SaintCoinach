using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class LeveAssignmentType : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public bool IsFaction { get { return AsBoolean("IsFaction"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }

        #endregion

        #region Constructors

        #region Constructor

        public LeveAssignmentType(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
