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
        Unknown19,
        Unknown20,
        Unknown21,
        BeastGauge,
        Unknown23,
        DreadwyrmAether,
        BloodGauge,
        Unknown26,
        NinkiGauge,
        Chakra,
        GreasedLightning,
        Aetherflow,
        AethertrailAttunement,
        Repertoire,
        AstrologianCard2,
        AstrologianCard3,
        AstrologianCard4,
        AstrologianCard5,
        AstrologianCard6,
        Unknown38,
        KenkiGauge,
        Unknown40,
        OathGauge,
        Unknown42,
        BalanceGauge,
        Unknown44,
        KenkiGauge2,
        FaerieGauge,
        DreadwyrmTrance,
        UnknownDragoon48,
        LilyAll,
        AstralFireOrUmbralIce,
        MP2,
        Ceruleum
    }

    public abstract class ClassJobActionBase : ActionBase {
        #region Properties

        public ClassJob ClassJob { get { return As<ClassJob>(); } }
        public ClassJobCategory ClassJobCategory { get { return As<ClassJobCategory>(); } }
        public int ClassJobLevel { get { return AsInt32("ClassJobLevel"); } }
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
