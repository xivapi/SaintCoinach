using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class TerritoryType : XivRow {
        #region Static

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
            }
        };

        #endregion

        #region Fields

        private WeatherRate _WeatherRate;

        #endregion

        #region Properties

        public string Name { get { return AsString("Name"); } }
        public string Bg { get { return AsString("Bg"); } }
        public Map Map { get { return As<Map>(); } }

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

        #region Constructor

        public TerritoryType(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
