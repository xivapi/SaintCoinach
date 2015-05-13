using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class BeastTribe : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public Text.XivString RelationName { get { return AsString("Name{Relation}"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        public ImageFile ReputationIcon { get { return AsImage("Icon{Reputation}"); } }

        #endregion

        #region Constructors

        #region Constructor

        public BeastTribe(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
