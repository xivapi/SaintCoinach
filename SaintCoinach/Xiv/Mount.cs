using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class Mount : XivRow {
        #region Properties

        public string Singular { get { return AsString("Singular"); } }
        public string Plural { get { return AsString("Plural"); } }
        public string Description { get { return AsString("Description"); } }
        public string GuideDescription { get { return AsString("Description{Enhanced}"); } }
        public string Tooltip { get { return AsString("Tooltip"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }

        #endregion

        #region Constructors

        #region Constructor

        public Mount(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Singular;
        }
    }
}
