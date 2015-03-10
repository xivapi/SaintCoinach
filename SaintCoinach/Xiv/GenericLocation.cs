using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class GenericLocation : ILocation {
        #region ILocation Members
        public double MapX { get; private set; }

        public double MapY { get; private set; }

        public PlaceName PlaceName { get; private set; }
        #endregion

        #region Constructors
        public GenericLocation(PlaceName placeName, double x, double y) {
            this.MapX = x;
            this.MapY = y;
            this.PlaceName = placeName;
        }
        #endregion
    }
}
