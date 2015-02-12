using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class Fate : XivRow {
        #region Properties

        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        public string Objective { get { return AsString("Objective"); } }
        public string StatusText0 { get { return AsString("StatusText", 0); } }
        public string StatusText1 { get { return AsString("StatusText", 1); } }
        public string StatusText2 { get { return AsString("StatusText", 2); } }
        public Level Level { get { return As<Level>(); } }
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
