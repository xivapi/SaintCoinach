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
        Unknown20, // Contents Actions?
        Unknown21,
        BeastGauge,
        Polyglot,
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
        Ceruleum,
        FourfoldFeather,
        Espirit,
        Cartridge,
        BloodLily,
        Lily,
        SealsAll,
        SoulVoice,
        Unknown60,
        Heat,
        Battery,
        Meditation
    }

    public abstract class ClassJobActionBase : ActionBase {
        #region Properties

        public ClassJob ClassJob { get { return As<ClassJob>(); } }
        public ClassJobCategory ClassJobCategory { get { return As<ClassJobCategory>(); } }
        public int ClassJobLevel { get { return AsInt32("ClassJobLevel"); } }

        #endregion

        #region Constructors

        protected ClassJobActionBase(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
