using System;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class Action : ClassJobActionBase {
        #region Properties

        public ActionCategory ActionCategory => As<ActionCategory>();
        public int SpellGroup => AsInt32("SpellGroup");
        public int Range => AsInt32("Range");
        public bool IsRoleAction => AsBoolean("IsRoleAction");
        public bool CanTargetSelf => AsBoolean("CanTargetSelf");
        public bool CanTargetParty => AsBoolean("CanTargetParty");
        public bool CanTargetFriendly => AsBoolean("CanTargetFriendly");
        public bool CanTargetHostile => AsBoolean("CanTargetHostile");
        public bool CanTargetDead => AsBoolean("CanTargetDead");
        public bool TargetArea => AsBoolean("TargetArea");
        public int EffectRange => AsInt32("EffectRange");
        public Action ComboFrom => As<Action>("Action{Combo}");
        public Status GainedStatus => As<Status>("Status{GainSelf}");
        public ActionCostType CostType => (ActionCostType)As<byte>("PrimaryCost{Type}");
        public int Cost => AsInt32("PrimaryCost{Value}");

        public TimeSpan CastTime {
            get { return TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond * 100 * AsInt32("Cast<100ms>")); }
        }

        public TimeSpan RecastTime {
            get { return TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond * 100 * AsInt32("Recast<100ms>")); }
        }


        #endregion

        #region Constructors

        #region Constructor

        public Action(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

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
