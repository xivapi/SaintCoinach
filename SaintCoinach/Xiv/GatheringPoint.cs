using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    using Ex.Relational;

    public class GatheringPoint : XivRow {
        #region Properties

        public GatheringPointBase Base { get { return As<GatheringPointBase>(); } }
        public GatheringCondition Condition { get { return As<GatheringCondition>(); } }
        public int ConditionValue { get { return AsInt32("GatheringConditionValue"); } }
        public GatheringPointBonusType BonusType { get { return As<GatheringPointBonusType>(); } }
        public int BonusValue { get { return AsInt32("GatheringPointBonus"); } }
        public TerritoryType TerritoryType { get { return As<TerritoryType>(); } }
        public PlaceName PlaceName { get { return As<PlaceName>(); } }

        #endregion

        #region Constructors

        public GatheringPoint(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
