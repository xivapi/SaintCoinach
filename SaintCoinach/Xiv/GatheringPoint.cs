using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    using Ex.Relational;

    public class GatheringPoint : XivRow {
        #region Fields

        private GatheringPointBonus[] _Bonuses;

        #endregion

        #region Properties

        public GatheringPointBase Base { get { return As<GatheringPointBase>(); } }
        public TerritoryType TerritoryType { get { return As<TerritoryType>(); } }
        public PlaceName PlaceName { get { return As<PlaceName>(); } }
        public GatheringPointBonus[] GatheringPointBonus { get { return _Bonuses ?? (_Bonuses = BuildGatheringPointBonus()); } }
        public GatheringSubCategory GatheringSubCategory => As<GatheringSubCategory>();
        #endregion

        #region Constructors

        public GatheringPoint(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build

        public GatheringPointBonus[] BuildGatheringPointBonus() {
            const int Count = 2;

            var bonuses = new List<GatheringPointBonus>();
            for (var i = 0; i < Count; ++i) {
                var bonus = As<GatheringPointBonus>(i);
                if (bonus.Key != 0)
                    bonuses.Add(bonus);
            }

            return bonuses.ToArray();
        }

        #endregion
    }
}
