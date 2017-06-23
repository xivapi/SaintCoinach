using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a territory (zone).
    /// </summary>
    public class TerritoryType : XivRow {
        #region Static

        /// <summary>
        ///     Mappings of special weather rates.
        /// </summary>
        private static Dictionary<int, WeatherRate> WeatherGroups;

        #endregion

        #region Fields

        /// <summary>
        ///     <see cref="WeatherRate" /> of the current territory.
        /// </summary>
        private WeatherRate _WeatherRate;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the name of the current territory.
        /// </summary>
        /// <value>The name of the current territory.</value>
        public Text.XivString Name { get { return AsString("Name"); } }

        /// <summary>
        ///     Gets the identifier used for the current territory.
        /// </summary>
        /// <value>The identifier used for the current territory.</value>
        public Text.XivString Bg { get { return AsString("Bg"); } }

        /// <summary>
        ///     Gets the <see cref="Map" /> of the current territory.
        /// </summary>
        /// <value>The <see cref="Map" /> of the current territory.</value>
        public Map Map { get { return As<Map>(); } }

        /// <summary>
        ///     Gets the <see cref="PlaceName" /> of the current territory.
        /// </summary>
        /// <value>The <see cref="PlaceName" /> of the current territory.</value>
        public PlaceName PlaceName { get { return As<PlaceName>("PlaceName"); } }

        /// <summary>
        ///     Gets the <see cref="PlaceName" /> of the region the current map is in.
        /// </summary>
        /// <value>The <see cref="PlaceName" /> of the region the current map is in.</value>
        public PlaceName RegionPlaceName { get { return As<PlaceName>("PlaceName{Region}"); } }

        /// <summary>
        ///     Gets the <see cref="PlaceName" /> of the zone the current territory is in.
        /// </summary>
        /// <value>The <see cref="PlaceName" /> of the zone the current territory is in.</value>
        public PlaceName ZonePlaceName { get { return As<PlaceName>("PlaceName{Zone}"); } }

        /// <summary>
        ///     Gets the <see cref="WeatherRate" /> of the current territory.
        /// </summary>
        /// <value>The <see cref="WeatherRate" /> of the current territory.</value>
        public WeatherRate WeatherRate {
            get {
                if (_WeatherRate != null) return _WeatherRate;

                if (WeatherGroups == null)
                    WeatherGroups = BuildWeatherGroups();

                var rateKey = AsInt32("WeatherRate");
                if (!WeatherGroups.TryGetValue(rateKey, out _WeatherRate))
                    _WeatherRate = Sheet.Collection.GetSheet<WeatherRate>()[rateKey];
                return _WeatherRate;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TerritoryType" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public TerritoryType(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        private Dictionary<int, WeatherRate> BuildWeatherGroups() {
            var map = new Dictionary<int, WeatherRate>();
            foreach (var weatherGroup in Sheet.Collection.GetSheet2("WeatherGroup")) {
                // Not sure what the other rows are used for.
                if (weatherGroup.Key != 0)
                    continue;

                map[weatherGroup.ParentRow.Key] = (WeatherRate)weatherGroup["WeatherRate"];
            }
            return map;
        }
    }
}
