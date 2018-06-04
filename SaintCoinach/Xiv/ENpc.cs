using System;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Xiv.Collections;

namespace SaintCoinach.Xiv {
    public class ENpc : ILocatable, IQuantifiableXivString {
        #region Fields

        private ENpcBase _Base;
        private ENpcResident _Resident;
        private ILocation[] _Locations;

        #endregion

        #region Properties

        public int Key { get; private set; }
        public ENpcCollection Collection { get; private set; }
        public ENpcResident Resident { get { return _Resident ?? (_Resident = Collection.ResidentSheet[Key]); } }
        public ENpcBase Base { get { return _Base ?? (_Base = Collection.BaseSheet[Key]); } }
        public Text.XivString Singular { get { return Resident.Singular; } }
        public Text.XivString Plural { get { return Collection.Collection.ActiveLanguage == Ex.Language.Japanese ? Singular : Resident.Plural; } }
        public Text.XivString Title { get { return Resident.Title; } }

        public IEnumerable<ILocation> Locations { get { return _Locations ?? (_Locations = BuildLocations()); } }

        #endregion

        #region Constructors

        public ENpc(ENpcCollection collection, int key) {
            Key = key;
            Collection = collection;
        }

        #endregion

        #region Build

        private Level[] BuildLevels() {
            return Collection.Collection.GetSheet<Level>().Where(_ => _.Object?.Key == Key).ToArray();
        }

        private ILocation[] BuildLocations() {
            var levelLocations = BuildLevels();

            var coll = Collection.Collection;
            if (!coll.IsLibraAvailable)
                return levelLocations.Cast<ILocation>().ToArray();

            var libraENpc = coll.Libra.ENpcResidents.FirstOrDefault(i => i.Key == this.Key);
            if (libraENpc == null)
                return levelLocations.ToArray();

            var locations = new List<ILocation>();
            locations.AddRange(levelLocations.Cast<ILocation>());

            var placeNames = coll.GetSheet<PlaceName>();
            var maps = coll.GetSheet<Map>();

            if (libraENpc.Coordinates != null) {
                foreach (var coord in libraENpc.Coordinates) {
                    var placeName = placeNames.First(i => i.Key == coord.Item1);

                    foreach (var c in coord.Item2) {
                        // Only add if no Level exists in the same area.
                        if (!levelLocations.Any(l => Math.Abs(l.MapX - c.X) < 1 && Math.Abs(l.MapY - c.Y) < 1 && (l.Map.LocationPlaceName == placeName || l.Map.PlaceName == placeName || l.Map.RegionPlaceName == placeName)))
                            locations.Add(new GenericLocation(placeName, c.X, c.Y));
                    }
                }
            }

            return locations.ToArray();
        }
        #endregion

        public override string ToString() {
            return Resident.Singular;
        }

        #region IQuantifiableName Members
        string IQuantifiable.Singular {
            get { return Singular; }
        }

        string IQuantifiable.Plural {
            get { return Plural; }
        }
        #endregion
    }
}
