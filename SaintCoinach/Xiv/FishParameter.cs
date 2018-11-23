using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class FishParameter : XivRow {
        #region Properties

        public Text.XivString Text => AsString("Text");
        public Item Item => As<Item>("Item");
        public bool IsInLog => AsBoolean("IsInLog");
        public bool TimeRestricted => AsBoolean("TimeRestricted");
        public bool WeatherRestricted => AsBoolean("WeatherRestricted");

        #endregion

        #region Constructors

        #region Constructor

        public FishParameter(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return string.Format("{0}", Item);
        }
    }
}
