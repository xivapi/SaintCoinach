using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class BuddyEquip : XivRow {
        #region Properties

        public string Name { get { return AsString("Name"); } }
        public string Singular { get { return AsString("Singular"); } }
        public string Plural { get { return AsString("Plural"); } }
        public GrandCompany GrandCompany { get { return As<GrandCompany>(); } }
        public ImageFile HeadIcon { get { return AsImage("Icon{Head}"); } }
        public ImageFile BodyIcon { get { return AsImage("Icon{Body}"); } }
        public ImageFile LegsIcon { get { return AsImage("Icon{Legs}"); } }

        #endregion

        #region Constructors

        #region Constructor

        public BuddyEquip(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
