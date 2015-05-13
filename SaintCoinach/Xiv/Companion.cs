using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class Companion : XivRow, IQuantifiableName {
        #region Properties

        public Text.XivString Singular { get { return AsString("Singular"); } }
        public Text.XivString Plural { get { return Sheet.Collection.ActiveLanguage == Ex.Language.Japanese ? Singular : AsString("Plural"); } }
        public Text.XivString Description { get { return AsString("Description"); } }
        public Text.XivString GuideDescription { get { return AsString("Description{Enhanced}"); } }
        public Text.XivString Tooltip { get { return AsString("Tooltip"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }

        #endregion

        #region Constructors

        #region Constructor

        public Companion(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Singular;
        }

        #region IQuantifiableName Members
        string IQuantifiableName.Singular {
            get { return Singular; }
        }

        string IQuantifiableName.Plural {
            get { return Plural; }
        }
        #endregion
    }
}
