using System.Collections.Generic;

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
        /// <remarks>
        ///     Just guesswork on my part, but it's been working out so far.
        /// </remarks>
        private static readonly Dictionary<int, int> WeatherGroups = new Dictionary<int, int> {
            {
                128, 0x00
            }, {
                129, 0x01
            }, {
                130, 0x02
            }, {
                131, 0x0E
            }, {
                132, 0x0F
            }, {
                133, 0x07
            }, {
                134, 0x08
            }, {
                135, 0x20
            }, {
                136, 0x22
            }, {
                137, 0x21
            }, {
                138, 0x30
            }, {
                139, 0x2F
            }
        };

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
        ///     Gets the <see cref="WeatherRate" /> of the current territory.
        /// </summary>
        /// <value>The <see cref="WeatherRate" /> of the current territory.</value>
        public WeatherRate WeatherRate {
            get {
                if (_WeatherRate != null) return _WeatherRate;

                var rateKey = AsInt32("WeatherRate");
                if (WeatherGroups.ContainsKey(rateKey))
                    rateKey = WeatherGroups[rateKey];
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
    }
}
