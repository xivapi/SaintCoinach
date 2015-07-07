using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    using Ex.Relational;

    public class GatheringPointBonus : XivRow {
        #region Properties

        public GatheringCondition Condition { get { return As<GatheringCondition>("Condition"); } }
        public int ConditionValue { get { return AsInt32("ConditionValue"); } }
        public GatheringPointBonusType BonusType { get { return As<GatheringPointBonusType>("BonusType"); } }
        public int BonusValue { get { return AsInt32("BonusValue"); } }

        #endregion

        #region Constructors

        public GatheringPointBonus(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        
        #endregion
    }
}
