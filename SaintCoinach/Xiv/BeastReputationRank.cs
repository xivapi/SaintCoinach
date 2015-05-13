using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class BeastReputationRank : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public int RequiredReputation { get { return AsInt32("RequiredReputation"); } }

        #endregion

        #region Constructors

        #region Constructor

        public BeastReputationRank(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
