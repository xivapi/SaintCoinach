using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class CraftAction : ClassJobActionBase {
        #region Constructors

        #region Constructor

        public CraftAction(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override Text.XivString Description => AsString("Description");
        public int Cost => AsInt32("Cost");
    }
}
