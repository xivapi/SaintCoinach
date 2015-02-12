using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public abstract class Item : XivRow {
        #region Properties

        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        public string Singular { get { return AsString("Singular"); } }
        public string Plural { get { return AsString("Plural"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        public int StackSize { get { return AsInt32("StackSize"); } }

        #endregion

        #region Constructors

        #region Constructor

        protected Item(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
