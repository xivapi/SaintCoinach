using System.Collections.Generic;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class MonsterNoteTarget : XivRow {
        #region Fields

        private Location[] _Locations;

        #endregion

        #region Properties

        public BNpcName BNpcName { get { return As<BNpcName>(); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        public IEnumerable<Location> Locations { get { return _Locations ?? (_Locations = BuildLocations()); } }

        #endregion

        #region Constructors

        #region Constructor

        public MonsterNoteTarget(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        private Location[] BuildLocations() {
            const int Count = 3;

            var locations = new List<Location>();

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

        public class Location {
            #region Properties

            public PlaceName ZonePlaceName { get; private set; }
            public PlaceName LocationPlaceName { get; private set; }

            #endregion

            #region Constructors

            #region Constructor

            public Location(PlaceName zonePlace, PlaceName locationPlace) {
                ZonePlaceName = zonePlace;
                LocationPlaceName = locationPlace;
            }

            #endregion

            #endregion
        }

        #endregion
    }
}
