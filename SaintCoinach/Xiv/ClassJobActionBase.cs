using System;
using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public enum ActionCostType : byte {
        NoCost = 0,
        HPPercent,
        Unknown2,
        MP,
        MPAll,
        TP,
        Unknown6,
        GP,
        CP,
        Unknown9,
        StatusAll,
        LimitBreakGauge,
        Unknown12,
        Unknown13,
        AstrologianCard,
        Unknown15,
        StatusAmount,
        Unknown17,
        TPAll,
        Unknown19
    }

    public abstract class ClassJobActionBase : ActionBase {
        #region Properties

        public ClassJob ClassJob { get { return As<ClassJob>(); } }
        public ClassJobCategory ClassJobCategory { get { return As<ClassJobCategory>(); } }
        public int Level { get { return AsInt32("Level"); } }
        public ActionCostType CostType {  get { return (ActionCostType)As<byte>("Cost{Type}"); } }
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

            return (int)(paramGrow.MpModifier * Cost);
        }
        #endregion
    }
}
