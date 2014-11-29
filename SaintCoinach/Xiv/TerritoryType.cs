using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class TerritoryType : XivRow {
        private static Dictionary<int, int> WeatherGroups = new Dictionary<int, int> {
            { 128, 0x00 },
            { 129, 0x01 },
            { 130, 0x02 },
            { 131, 0x0E },
            { 132, 0x0F },
            { 133, 0x07 },
            { 134, 0x08 },
            { 135, 0x20 },
            { 136, 0x22 },
            { 137, 0x21 },
        };

        #region Fields
        private WeatherRate _WeatherRate = null;
        #endregion

        #region Properties
        public string Name { get { return AsString("Name"); } }
        public string Bg { get { return AsString("Bg"); } }
        //public Map Map { get { return As<Map>(); } }
        public WeatherRate WeatherRate {
            get {
                if (_WeatherRate == null) {
                    var rateKey = AsInt32("WeatherRate");
                    if (WeatherGroups.ContainsKey(rateKey))
                        rateKey = WeatherGroups[rateKey];
                    _WeatherRate = Sheet.Collection.GetSheet<WeatherRate>()[rateKey];
                }
                return _WeatherRate;
            }
        }
        #endregion

        #region Constructor
        public TerritoryType(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}
