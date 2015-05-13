using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class Emote : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public EmoteCategory EmoteCategory { get { return As<EmoteCategory>(); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        public LogMessage TargetedLogMessage { get { return As<LogMessage>("LogMessage{Targeted}"); } }
        public LogMessage UntargetedLogMessage { get { return As<LogMessage>("LogMessage{Untargeted}"); } }

        #endregion

        #region Constructors

        #region Constructor

        public Emote(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
