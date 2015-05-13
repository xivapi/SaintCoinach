using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class AchievementCategory : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public AchievementKind AchievementKind { get { return As<AchievementKind>(); } }

        #endregion

        #region Constructors

        #region Constructor

        public AchievementCategory(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
