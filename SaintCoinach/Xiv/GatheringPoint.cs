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
        public TerritoryType TerritoryType { get { return As<TerritoryType>(); } }
        public PlaceName PlaceName { get { return As<PlaceName>(); } }
        public GatheringPointBonus GatheringPointBonus { get { return As<GatheringPointBonus>(); } }
        
        #endregion

        #region Constructors

        public GatheringPoint(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
