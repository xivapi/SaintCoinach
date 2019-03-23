using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Class representing a combination of <see cref="BNpcName"/> and <see cref="BNpcBase"/>.
    /// </summary>
    /// <remarks>
    /// This class relies on information provided by Libra Eorzea.
    /// </remarks>
    public class BNpc : ILocatable, IItemSource {
        #region Fields
        private Libra.BNpcName _LibraRow;
        private BNpcLocation[] _Locations;
        private Item[] _Items;
        private InstanceContent[] _InstanceContents;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the key of the current BNpc.
        /// </summary>
        /// <value>The key of the current BNpc.</value>
        /// <remarks>
        /// This value is ((<c>Base.Key</c> * 10000000000) + <c>Name.Key</c>).
        /// </remarks>
        public long Key { get; private set; }

        public string FullKey => Base.Key + "-" + Name.Key;

        /// <summary>
        /// Gets the <see cref="BNpcBase"/> of the current BNpc.
        /// </summary>
        /// <value>The <see cref="BNpcBase"/> of the current BNpc.</value>
        public BNpcBase Base { get; private set; }

        /// <summary>
        /// Gets the <see cref="BNpcName"/> of the current BNpc.
        /// </summary>
        /// <value>The <see cref="BNpcName"/> of the current BNpc.</value>
        public BNpcName Name { get; private set; }

        /// <summary>
        /// Gets the <see cref="Collections.BNpcCollection"/> of the current BNpc.
        /// </summary>
        /// <value>The <see cref="Collections.BNpcCollection"/> of the current BNpc.</value>
        public Collections.BNpcCollection Collection { get; private set; }

        /// <summary>
        /// Gets the locations of the current object.
        /// </summary>
        /// <value>The locations of the current object.</value>
        IEnumerable<ILocation> ILocatable.Locations { get { return this.Locations; } }

        /// <summary>
        /// Gets the locations of the current object.
        /// </summary>
        /// <value>The locations of the current object.</value>
        public IEnumerable<BNpcLocation> Locations { get { return _Locations ?? (_Locations = BuildLocations(_LibraRow)); } }

        /// <summary>
        /// Gets the items dropped by the current object.
        /// </summary>
        /// <value>The items dropped by the current object.</value>
        public IEnumerable<Item> Items { get { return _Items ?? (_Items = BuildItems(_LibraRow)); } }

        /// <summary>
        /// Gets the <see cref="InstanceContent"/>s the current BNpc appears in.
        /// </summary>
        /// <value>The <see cref="InstanceContent"/>s the current BNpc appears in.</value>
        public IEnumerable<InstanceContent> InstanceContents { get { return _InstanceContents ?? (_InstanceContents = BuildInstanceContents(_LibraRow)); } }

        #endregion

        #region Constructor
        public BNpc(Collections.BNpcCollection collection, Libra.BNpcName libra) {
            this.Collection = collection;
            _LibraRow = libra;
            this.Key = libra.Key;
            this.Base = collection.BaseSheet[(int)libra.BaseKey];
            this.Name = collection.NameSheet[(int)libra.NameKey];
        }
        #endregion


        #region Build
        private BNpcLocation[] BuildLocations(Libra.BNpcName libraRow) {
            var values = new List<BNpcLocation>();
            var placeNames = Collection.Collection.GetSheet<PlaceName>();

            foreach (var srcRegion in libraRow.Regions) {
                var region = placeNames[srcRegion.Item1];

                foreach (var srcZone in srcRegion.Item2) {
                    var zone = placeNames[srcZone.Item1];
                    int lvMin, lvMax;
                    if (srcZone.Item2.Length > 0) {
                        lvMin = srcZone.Item2.Min();
                        lvMax = srcZone.Item2.Max();
                    } else
                        lvMax = lvMin = 0;

                    values.Add(new BNpcLocation(region, zone, lvMin, lvMax));
                }
            }

            return values.ToArray();
        }

        private Item[] BuildItems(Libra.BNpcName libraRow) {
            var values = new List<Item>();
            var items = Collection.Collection.GetSheet<Item>();

            foreach (var srcItem in libraRow.Items) {
                values.Add(items[srcItem]);
            }

            return values.ToArray();
        }

        private InstanceContent[] BuildInstanceContents(Libra.BNpcName libraRow) {
            var values = new List<InstanceContent>();
            var instanceContents = Collection.Collection.GetSheet<InstanceContent>();

            foreach (var srcContent in libraRow.InstanceContents) {
                values.Add(instanceContents[srcContent]);
            }

            return values.ToArray();
        }
        #endregion

        public override string ToString() {
            return Name.Singular;
        }
    }
}
