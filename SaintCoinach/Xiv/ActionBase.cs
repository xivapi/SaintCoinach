using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public abstract class ActionBase : XivRow {
        #region Properties

        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        public ClassJob ClassJob { get { return As<ClassJob>(); } }
        public ClassJobCategory ClassJobCategory { get { return As<ClassJobCategory>(); } }
        public int Level { get { return AsInt32("Level"); } }
        public int Cost { get { return AsInt32("Cost"); } }

        #endregion

        #region Constructors

        #region Constructor

        protected ActionBase(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
