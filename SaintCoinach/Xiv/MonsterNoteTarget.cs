using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class MonsterNoteTarget : XivRow {
        #region Location class
        public class Location {
            #region Fields
            private PlaceName _ZonePlaceName;
            private PlaceName _LocationPlaceName;
            #endregion

            #region Properties
            public PlaceName ZonePlaceName { get { return _ZonePlaceName; } }
            public PlaceName LocationPlaceName { get { return _LocationPlaceName; } }
            #endregion

            #region Constructor
            public Location(PlaceName zonePlace, PlaceName locationPlace) {
                _ZonePlaceName = zonePlace;
                _LocationPlaceName = locationPlace;
            }
            #endregion
        }
        #endregion

        #region Fields
        private Location[] _Locations;
        #endregion

        #region Properties
        public BNpcName BNpcName { get { return As<BNpcName>(); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        public IEnumerable<Location> Locations { get { return _Locations ?? (_Locations = BuildLocations()); } }
        #endregion

        #region Constructor
        public MonsterNoteTarget(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
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
    }
}