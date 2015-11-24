using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class Fate : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public Text.XivString Description { get { return AsString("Description"); } }
        public Text.XivString Objective { get { return AsString("Objective"); } }
        public Text.XivString StatusText0 { get { return AsString("StatusText", 0); } }
        public Text.XivString StatusText1 { get { return AsString("StatusText", 1); } }
        public Text.XivString StatusText2 { get { return AsString("StatusText", 2); } }
        public int CharacterLevel { get { return AsInt32("CLevel"); } }
        public int MaximumCharacterLevel { get { return AsInt32("CLevel{Max}"); } }
        public EventItem EventItem { get { return As<EventItem>(); } }
        public ImageFile ObjectiveIcon { get { return AsImage("Icon{Objective}"); } }
        public ImageFile MapIcon { get { return AsImage("Icon{Map}"); } }

        #endregion

        #region Constructors

        #region Constructor

        public Fate(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
