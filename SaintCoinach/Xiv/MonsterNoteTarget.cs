using System;
using System.Linq;
using System.Collections.Generic;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class MonsterNoteTarget : XivRow, ILocatable {
        #region Fields

        private IEnumerable<ILocation> _Locations;

        #endregion

        #region Properties

        public BNpcName BNpcName { get { return As<BNpcName>(); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        public IEnumerable<ILocation> Locations { get { return _Locations ?? (_Locations = BuildLocations()); } }

        #endregion

        #region Constructors

        #region Constructor

        public MonsterNoteTarget(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        private IEnumerable<ILocation> BuildLocations() {
            if (Sheet.Collection.IsLibraAvailable)
                return BNpcName.Locations;

            const int Count = 3;

            var locations = new List<ILocation>();

            var maps = Sheet.Collection.GetSheet<Map>();

            for (var i = 0; i < Count; ++i) {
                var zone = As<PlaceName>("PlaceName{Zone}", i);
                var location = As<PlaceName>("PlaceName{Location}", i);

                if (zone.Key != 0)
                    locations.Add(new Location(zone, location));
            }

            return locations.ToArray();
        }

        #endregion

        #region Location class

        public class Location : ILocation {
            #region Properties

            double ILocation.MapX { get { return double.NaN; } }
            double ILocation.MapY { get { return double.NaN; } }

            public PlaceName PlaceName { get; private set; }
            public PlaceName LocationPlaceName { get; private set; }

            #endregion

            #region Constructors

            public Location(PlaceName zonePlace, PlaceName locationPlace) {
                PlaceName = zonePlace;
                LocationPlaceName = locationPlace;
            }

            #endregion
        }

        #endregion
    }
}
