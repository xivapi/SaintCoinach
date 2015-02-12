using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class MainCommand : XivRow {
        #region Properties

        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        public MainCommandCategory MainCommandCategory { get { return As<MainCommandCategory>(); } }

        #endregion

        #region Constructors

        #region Constructor

        public MainCommand(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
