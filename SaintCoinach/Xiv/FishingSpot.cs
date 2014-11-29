using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class FishingSpot : XivRow {
        #region Fields
        private Item[] _Items = null;
        #endregion

        #region Properties
        public int Level { get { return AsInt32("Level"); } }
        public string OnReachBigFish { get { return AsString("BigFish{OnReach}"); } }
        public string OnEndBigFish { get { return AsString("BigFish{OnEnd}"); } }
        public FishingSpotCategory FishingSpotCategory { get { return As<FishingSpotCategory>(); } }
        public TerritoryType TerritoryType { get { return As<TerritoryType>(); } }
        public int X { get { return AsInt32("X"); } }
        public int Z { get { return AsInt32("Z"); } }
        public int Radius { get { return AsInt32("Radius"); } }
        public PlaceName PlaceName { get { return As<PlaceName>(); } }
        #endregion

        #region Constructor
        public FishingSpot(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private Item[] BuildItems() {
            const int Count = 10;

            var items = new List<Item>();
            for (var i = 0; i < Count; ++i) {
                var item = As<Item>("Item", i);
                if (item.Key != 0)
                    items.Add(item);
            }

            return items.ToArray();
        }
        #endregion

        public override string ToString() {
            return string.Format("{0}", PlaceName);
        }
    }
}