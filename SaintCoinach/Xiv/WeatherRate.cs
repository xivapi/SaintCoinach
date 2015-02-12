using System;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class WeatherRate : XivRow {
        #region Static

        public static readonly TimeSpan WeatherChangeInterval = TimeSpan.FromHours(8);

        #endregion

        #region Fields

        private readonly Weather[] _PossibleWeathers;
        private readonly Tuple<int, Weather>[] _WeatherRates;

        #endregion

        #region Properties

        public IEnumerable<Weather> PossibleWeathers { get { return _PossibleWeathers; } }

        #endregion

        #region Constructors

        #region Constructor

        public WeatherRate(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) {
            const int Count = 8;

            var w = new List<Weather>();
            var wr = new List<Tuple<int, Weather>>();
            var min = 0;
            for (var i = 0; i < Count; ++i) {
                var suffix = string.Format("[{0}]", i);

                var weather = (Weather)this["Weather" + suffix];
                var rate = Convert.ToInt32(this["Rate" + suffix]);

                w.Add(weather);
                wr.Add(Tuple.Create(min + rate, weather));

                min += rate;
            }
            _PossibleWeathers = w.Distinct().ToArray();
            _WeatherRates = wr.ToArray();
        }

        #endregion

        #endregion

        #region Forecast

        public Weather Forecast(EorzeaDateTime time) {
            var target = CalculateTarget(time);

            return _WeatherRates.Where(_ => target < _.Item1).Select(_ => _.Item2).FirstOrDefault();
        }

        private static int CalculateTarget(EorzeaDateTime time) {
            var unix = time.GetUnixTime();
            // Get Eorzea hour for weather start
            // Do the magic 'cause for calculations 16:00 is 0, 00:00 is 8 and 08:00 is 16
            var bell = unix / 175;
            var increment = (uint)(bell + 8 - bell % 8) % 24;

            // Take Eorzea days since unix epoch
            var totalDays = (uint)(unix / 4200);

            var calcBase = (totalDays * 0x64) + increment;

            var step1 = (calcBase << 0xB) ^ calcBase;
            var step2 = (step1 >> 8) ^ step1;

            return (int)(step2 % 0x64);
        }

        #endregion
    }
}
