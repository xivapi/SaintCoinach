using System;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class enabling the prediction of weather in Eorzea.
    /// </summary>
    public class WeatherRate : XivRow {
        #region Static

        /// <summary>
        ///     Interval in which the <see cref="Weather" /> can change.
        /// </summary>
        public static readonly TimeSpan WeatherChangeInterval = TimeSpan.FromHours(8);

        #endregion

        #region Fields

        /// <summary>
        ///     <see cref="Weather" />s possible for the current group.
        /// </summary>
        private readonly Weather[] _PossibleWeathers;

        /// <summary>
        ///     Helper field for <see cref="Forecast" />. <c>Item1</c> is the maximum predication value for <c>Item2</c> to happen.
        /// </summary>
        private readonly Tuple<int, Weather>[] _WeatherRates;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the <see cref="Weather" />s possible for the current group.
        /// </summary>
        /// <value>The <see cref="Weather" />s possible for the current group.</value>
        public IEnumerable<Weather> PossibleWeathers { get { return _PossibleWeathers; } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeatherRate" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
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

        #region Forecast

        /// <summary>
        ///     Forecast the <see cref="Weather" /> for the current location at a specific <see cref="EorzeaDateTime" />.
        /// </summary>
        /// <param name="time"><see cref="EorzeaDateTime" /> for which to forecast the weather.</param>
        /// <returns>The <see cref="Weather" /> at the current location at <c>time</c>.</returns>
        public Weather Forecast(EorzeaDateTime time) {
            var target = CalculateTarget(time);

            return _WeatherRates.Where(_ => target < _.Item1).Select(_ => _.Item2).FirstOrDefault();
        }

        /// <summary>
        ///     Calculate the value used for the <see cref="Forecast" /> at a specific <see cref="EorzeaDateTime" />.
        /// </summary>
        /// <param name="time"><see cref="EorzeaDateTime" /> for which to calculate the value.</param>
        /// <returns>The value from 0..99 (inclusive) calculated based on <c>time</c>.</returns>
        private static int CalculateTarget(EorzeaDateTime time) {
            var unix = time.GetUnixTime();
            // Get Eorzea hour for weather start
            var bell = unix / 175;
            // Do the magic 'cause for calculations 16:00 is 0, 00:00 is 8 and 08:00 is 16
            var increment = ((uint)(bell + 8 - (bell % 8))) % 24;

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
