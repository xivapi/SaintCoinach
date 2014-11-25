using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class WeatherRate : XivRow {
        #region Fields
        private Weather[] _PossibleWeathers;
        private Tuple<int, Weather>[] _WeatherRates;
        #endregion

        #region Constructor
        public WeatherRate(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) {
            const int Count = 8;

            var w = new List<Weather>();
            var wr = new List<Tuple<int, Weather>>();
            int min = 0;
            for (int i = 0; i < Count; ++i) {
                var suffix = string.Format("[{0}]", i);

                var weather = (Weather)this["Weather" + suffix];
                var rate = Convert.ToInt32(this["Rate" + suffix]);

                w.Add(weather);
                wr.Add(Tuple.Create(min + rate, weather));

                min += rate;
            }
            _PossibleWeathers = w.ToArray();
            _WeatherRates = wr.ToArray();
        }
        #endregion

        #region Forecast
        public Weather Forecast(EorzeaDateTime time) {
            var target = CalculateTarget(time);

            return _WeatherRates.Where(_ => target < _.Item1).Select(_ => _.Item2).FirstOrDefault();
        }
        private static int CalculateTarget(EorzeaDateTime time) {
            const int Mod = 0x64;

            // Get Eorzea hour for weather start
            // Do the magic 'cause for calculations 16:00 is 0, 00:00 is 8 and 08:00 is 16
            uint increment = (uint)(time.Bell + 8 - time.Bell % 8) % 24;
            // Take Eorzea days since unix epoch
            uint totalDays = (uint)(time.GetUnixTime() / 4200);

            uint calcBase = (totalDays * 0x64) + increment;

            var step1 = (calcBase << 0xB) ^ calcBase;
            var step2 = (step1 >> 8) ^ step1;

            return (int)(step2 % Mod);
        }
        #endregion
    }
}
