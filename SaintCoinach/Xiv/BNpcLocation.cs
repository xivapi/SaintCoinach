using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class BNpcLocation : ILocation {
        #region Properties

        double ILocation.MapX { get { return double.NaN; } }
        double ILocation.MapY { get { return double.NaN; } }

        public PlaceName RegionPlaceName{ get; private set; }
        public PlaceName PlaceName { get; private set; }

        public int MinimumLevel { get; private set; }
        public int MaximumLevel { get; private set; }

        #endregion

        #region Constructors
        public BNpcLocation(PlaceName regionPlaceName, PlaceName placeName, int minimumLevel, int maximumLevel) {
            this.RegionPlaceName = regionPlaceName;
            this.PlaceName = placeName;
            this.MinimumLevel = minimumLevel;
            this.MaximumLevel = maximumLevel;
        }
        #endregion
    }
}
