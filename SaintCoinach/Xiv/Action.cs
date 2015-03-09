using System;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class Action : ClassJobActionBase {
        #region Properties

        public ActionCategory ActionCategory { get { return As<ActionCategory>(); } }
        public int SpellGroup { get { return AsInt32("SpellGroup"); } }
        public int Range { get { return AsInt32("Range"); } }
        public bool CanTargetSelf { get { return AsBoolean("CanTargetSelf"); } }
        public bool CanTargetParty { get { return AsBoolean("CanTargetParty"); } }
        public bool CanTargetFriendly { get { return AsBoolean("CanTargetFriendly"); } }
        public bool CanTargetHostile { get { return AsBoolean("CanTargetHostile"); } }
        public bool CanTargetDead { get { return AsBoolean("CanTargetDead"); } }
        public bool TargetArea { get { return AsBoolean("TargetArea"); } }
        public int EffectRange { get { return AsInt32("EffectRange"); } }
        public Status RequiredStatus { get { return As<Status>("Status{Required}"); } }
        public Action ComboFrom { get { return As<Action>("Action{Combo}"); } }

        public TimeSpan CastTime {
            get { return TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond * 100 * AsInt32("Cast<100ms>")); }
        }

        public TimeSpan RecastTime {
            get { return TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond * 100 * AsInt32("Recast<100ms>")); }
        }

        public Status GainedStatus { get { return As<Status>("Status{GainSelf}"); } }

        #endregion

        #region Constructors

        #region Constructor

        public Action(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
