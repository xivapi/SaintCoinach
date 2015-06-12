using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class BuddyEquip : XivRow, IQuantifiableXivString {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public Text.XivString Singular { get { return AsString("Singular"); } }
        public Text.XivString Plural { get { return Sheet.Collection.ActiveLanguage == Ex.Language.Japanese ? Singular : AsString("Plural"); } }
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

        #region IQuantifiableName Members
        string IQuantifiable.Singular {
            get { return Singular; }
        }

        string IQuantifiable.Plural {
            get { return Plural; }
        }
        #endregion
    }
}
