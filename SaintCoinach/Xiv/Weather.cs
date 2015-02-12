using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class Weather : XivRow {
        #region Properties

        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }

        #endregion

        #region Constructors

        #region Constructor

        public Weather(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region ToString

        public override string ToString() {
            return Name;
        }

        #endregion
    }
}
