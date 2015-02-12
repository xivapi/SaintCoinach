using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class BuddyAction : XivRow {
        #region Properties

        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        public ImageFile StatusIcon { get { return AsImage("Icon{Status}"); } }

        #endregion

        #region Constructors

        #region Constructor

        public BuddyAction(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
