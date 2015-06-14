using System;
using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public abstract class ClassJobActionBase : ActionBase {
        #region Properties

        public ClassJob ClassJob { get { return As<ClassJob>(); } }
        public ClassJobCategory ClassJobCategory { get { return As<ClassJobCategory>(); } }
        public int Level { get { return AsInt32("Level"); } }
        public int Cost { get { return AsInt32("Cost"); } }

        #endregion

        #region Constructors

        protected ClassJobActionBase(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Helper
        public int GetMpCost(int level) {
            var paramGrowSheet = Sheet.Collection.GetSheet<ParamGrow>();
            if (!paramGrowSheet.ContainsRow(level))
                return 0;
            var paramGrow = paramGrowSheet[level];

            return (int)Math.Round(paramGrow.MpCostFactor * Cost, MidpointRounding.AwayFromZero);
        }
        #endregion
    }
}
