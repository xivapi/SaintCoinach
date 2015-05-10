using System;

namespace SaintCoinach {
    /// <summary>
    ///     Class for operations using the Eorzean calendar and time.
    /// </summary>
    /// <remarks>
    ///     One Eorzean day is 70 real-world minutes long, with zero starting at the Unix epoch (1. Jan 1970, 00:00.00).
    ///     The Eorzean calendar is divided into <see cref="Year" />, <see cref="Moon" />, <see cref="Sun" />,
    ///     <see cref="Bell" /> and <see cref="Minute" />.
    ///     One <see cref="Year" /> is 12 <see cref="Moon" />s.
    ///     One <see cref="Moon" /> is 32 <see cref="Sun" />s.
    ///     One <see cref="Bell" /> is 24 <see cref="Minute" />s.
    /// </remarks>
    public partial class EorzeaDateTime {
        #region Static

        /// <summary>
        ///     Factor used to convert from real to Eorzean time.
        /// </summary>
        /// <remarks>
        ///     60 * 24 Eorzean minutes (one day) per 70 real-world minutes.
        /// </remarks>
        public const double RealToEorzeanFactor = (60.0 * 24.0) / 70.0;

        /// <summary>
        ///     Interval in which the value of <see cref="Now" /> is updated.
        /// </summary>
        private const long NowUpdateInterval = TimeSpan.TicksPerSecond;

        /// <summary>
        ///     <see cref="DateTime" /> of the Unix epoch.
        /// </summary>
        public static readonly DateTime Zero = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        ///     Last update of the value of <see cref="EorzeaDateTime.Now" /> in ticks.
        /// </summary>
        private static long _LastNowUpdate = DateTime.UtcNow.Ticks;

        /// <summary>
        ///     Current value of <see cref="Now" />.
        /// </summary>
        private static EorzeaDateTime _Now = new EorzeaDateTime();

        #endregion

        #region Fields

        /// <summary>
        ///     Current bell (hour) in Eorzean time.
        /// </summary>
        private int _Bell;

        /// <summary>
        ///     Current minute in Eorzean time.
        /// </summary>
        private int _Minute;

        /// <summary>
        ///     Current moon (month) in Eorzean time.
        /// </summary>
        private int _Moon;

        /// <summary>
        ///     Current sun (day) in Eorzean time.
        /// </summary>
        private int _Sun;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the current time in the Eorzean calendar.
        /// </summary>
        /// <value>The current time in the Eorzean calendar.</value>
        public static EorzeaDateTime Now {
            get {
                var nt = DateTime.UtcNow.Ticks;
                var d = nt - _LastNowUpdate;
                if (d <= NowUpdateInterval) return _Now;

                _Now = new EorzeaDateTime();
                _LastNowUpdate = nt;
                return _Now;
            }
        }

        /// <summary>
        ///     Gets or sets the current minute in the Eorzean calendar.
        /// </summary>
        /// <remarks>
        ///     Setting values outside the 0..59 range will be adjusted accordingly and carry over to <see cref="Bell" />.
        /// </remarks>
        /// <value>The current minute in the Eorzean calendar.</value>
        public int Minute {
            get { return _Minute; }
            set {
                _Minute = value;
                while (_Minute < 0) {
                    _Minute += 60;
                    Bell--;
                }
                while (_Minute >= 60) {
                    _Minute -= 60;
                    Bell++;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the current bell (hour) in the Eorzean calendar.
        /// </summary>
        /// <remarks>
        ///     Setting values outside the 0..23 range will be adjusted accordingly and carry over to <see cref="Sun" />.
        /// </remarks>
        /// <value>The current bell (hour) in the Eorzean calendar.</value>
        public int Bell {
            get { return _Bell; }
            set {
                _Bell = value;
                while (_Bell < 0) {
                    _Bell += 24;
                    Sun--;
                }
                while (_Bell >= 24) {
                    _Bell -= 24;
                    Sun++;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the current sun (day) in the Eorzean calendar.
        /// </summary>
        /// <remarks>
        ///     Setting values outside the 1..32 range will be adjusted accordingly and carry over to <see cref="Moon" />.
        /// </remarks>
        /// <value>The current sun (day) in the Eorzean calendar.</value>
        public int Sun {
            get { return _Sun; }
            set {
                _Sun = value;
                while (_Sun < 1) {
                    _Sun += 32;
                    Moon--;
                }
                while (_Sun > 32) {
                    _Sun -= 32;
                    Moon++;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the current moon (month) in the Eorzean calendar.
        /// </summary>
        /// <remarks>
        ///     Setting values outside the 1..12 range will be adjusted accordingly and carry over to <see cref="Year" />.
        /// </remarks>
        /// <value>The current moon (month) in the Eorzean calendar.</value>
        public int Moon {
            get { return _Moon; }
            set {
                _Moon = value;
                while (_Moon < 1) {
                    _Moon += 12;
                    Year--;
                }
                while (_Moon > 12) {
                    _Moon -= 12;
                    Year++;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the current year in the Eorzean calendar.
        /// </summary>
        /// <remarks>
        ///     This value is not actually used anywhere in the game, it is only preserved here to make conversion to real-time
        ///     possible.
        /// </remarks>
        /// <value>The current year in the Eorzean calendar.</value>
        public int Year { get; set; }

        /// <summary>
        ///     Gets the total amount of minutes in the Eorzean calendar since zero.
        /// </summary>
        /// <value>The total amount of minutes in the Eorzean calendar since zero.</value>
        public double TotalMinutes {
            get { return Minute + (60.0 * (Bell + (24.0 * (Sun + (32.0 * (Moon + (12.0 * Year))))))); }
        }

        /// <summary>
        ///     Gets the total amount of bells (hours) in the Eorzean calendar since zero.
        /// </summary>
        /// <value>The total amount of bells (hours) in the Eorzean calendar since zero.</value>
        public double TotalBells {
            get { return Bell + (Minute / 60.0) + (24 * (Sun + (32.0 * (Moon + (12.0 * Year))))); }
        }

        /// <summary>
        ///     Gets the total amount of suns (days) in the Eorzean calendar since zero.
        /// </summary>
        /// <value>The total amount of suns (days) in the Eorzean calendar since zero.</value>
        public double TotalSuns {
            get { return Sun + ((Bell + (Minute / 60.0)) / 24.0) + (32 * (Moon + (12.0 * Year))); }
        }

        /// <summary>
        ///     Gets the total amount of moons (months) in the Eorzean calendar since zero.
        /// </summary>
        /// <value>The total amount of moons (months) in the Eorzean calendar since zero.</value>
        public double TotalMoons {
            get { return Moon + ((Sun + ((Bell + (Minute / 60.0)) / 24.0)) / 32.0) + (12.0 * Year); }
        }

        /// <summary>
        ///     Gets the total amount of years in the Eorzean calendar since zero.
        /// </summary>
        /// <value>The total amount of years in the Eorzean calendar since zero.</value>
        public double TotalYears {
            get { return Year + ((Moon + ((Sun + ((Bell + (Minute / 60.0)) / 24.0)) / 32.0)) / 12.0); }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EorzeaDateTime" /> class that represents the same time and date as
        ///     another <see cref="EorzeaDateTime" />.
        /// </summary>
        /// <param name="source"><see cref="EorzeaDateTime" /> whose values to copy.</param>
        public EorzeaDateTime(EorzeaDateTime source) {
            Minute = source.Minute;
            Bell = source.Bell;
            Sun = source.Sun;
            Moon = source.Moon;
            Year = source.Year;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EorzeaDateTime" /> class with the current time and date.
        /// </summary>
        public EorzeaDateTime() : this(DateTime.UtcNow) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EorzeaDateTime" /> class for the specified Unix timestamp.
        /// </summary>
        /// <param name="unixTime">Unix timestamp of the time and date.</param>
        public EorzeaDateTime(int unixTime) : this((long)unixTime) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EorzeaDateTime" /> class for the specified Unix timestamp.
        /// </summary>
        /// <param name="unixTime">Unix timestamp of the time and date.</param>
        public EorzeaDateTime(long unixTime) {
            SetUnixTime(unixTime);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EorzeaDateTime" /> class with the Eorzean equivalent for
        ///     <see cref="DateTime" />.
        /// </summary>
        /// <param name="time"><see cref="DateTime" /> to convert to Eorzean time.</param>
        public EorzeaDateTime(DateTime time) {
            SetRealTime(time);
        }

        #endregion

        /// <summary>
        ///     Convert the current date and time to a <see cref="TimeSpan" />.
        /// </summary>
        /// <returns>Returns the <see cref="TimeSpan" /> of elapsed time since zero in the Eorzean calendar.</returns>
        public TimeSpan GetTimeSpan() {
            return new TimeSpan(Sun + (((Year * 12) + Moon) * 32), Bell, Minute, 0, 0);
        }

        /// <summary>
        ///     Get the Unix timestamp.
        /// </summary>
        /// <returns>Returns the Unix timestamp.</returns>
        public long GetUnixTime() {
            var years = Year;
            var moons = (years * 12.0) + Moon - 1;
            var suns = (moons * 32.0) + Sun - 1;
            var bells = (suns * 24.0) + Bell;
            var minutes = (bells * 60.0) + Minute;
            var seconds = minutes * 60.0;

            return (long)(seconds / RealToEorzeanFactor);
        }

        /// <summary>
        ///     Set the Unix timestamp.
        /// </summary>
        /// <param name="time">Unix timestamp to use.</param>
        /// <returns>Returns self.</returns>
        public EorzeaDateTime SetUnixTime(long time) {
            var eorzeaSeconds = time * RealToEorzeanFactor;

            SetEorzeaTime(eorzeaSeconds);

            return this;
        }

        /// <summary>
        ///     Set the Eorzean time using the total elapsed seconds since zero.
        /// </summary>
        /// <param name="eorzeaSeconds">Elapsed seconds since zero.</param>
        private void SetEorzeaTime(double eorzeaSeconds) {
            var minutes = eorzeaSeconds / 60;
            var bells = minutes / 60;
            var suns = bells / 24;
            var moons = suns / 32;
            var years = moons / 12;

            Minute = (int)(minutes % 60);
            Bell = (int)(bells % 24);
            Sun = (int)(suns % 32) + 1;
            Moon = (int)(moons % 12) + 1;
            Year = (int)years;
        }

        /// <summary>
        ///     Get the real-world <see cref="DateTime" /> of this <see cref="EorzeaDateTime" />.
        /// </summary>
        /// <returns>Returns the <see cref="DateTime" /> of this <see cref="EorzeaDateTime" />.</returns>
        public DateTime GetRealTime() {
            return Zero + TimeSpan.FromSeconds(GetUnixTime());
        }

        /// <summary>
        ///     Set the value of this <see cref="EorzeaDateTime" /> from a real-world <see cref="DateTime" />.
        /// </summary>
        /// <param name="time"><see cref="DateTime" /> to convert.</param>
        /// <returns>Returns self.</returns>
        public EorzeaDateTime SetRealTime(DateTime time) {
            var utc = time.ToUniversalTime();
            var fromZero = utc - Zero;
            return SetUnixTime((long)fromZero.TotalSeconds);
        }

        /// <summary>
        ///     Increase the time and date of a <see cref="EorzeaDateTime" /> by a <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="lh"><see cref="EorzeaDateTime" /> to use as base.</param>
        /// <param name="rh"><see cref="TimeSpan" /> to add to the <see cref="EorzeaDateTime" />.</param>
        /// <returns>Returns a new <see cref="EorzeaDateTime" /> object with the resulting time and date.</returns>
        public static EorzeaDateTime operator+(EorzeaDateTime lh, TimeSpan rh) {
            var copy = lh.Clone();
            copy.Minute += (int)rh.TotalMinutes;
            return copy;
        }

        /// <summary>
        ///     Decrease the time and date of a <see cref="EorzeaDateTime" /> by a <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="lh"><see cref="EorzeaDateTime" /> to use as base.</param>
        /// <param name="rh"><see cref="TimeSpan" /> to subtract to the <see cref="EorzeaDateTime" />.</param>
        /// <returns>Returns a new <see cref="EorzeaDateTime" /> object with the resulting time and date.</returns>
        public static EorzeaDateTime operator-(EorzeaDateTime lh, TimeSpan rh) {
            var copy = lh.Clone();
            copy.Minute -= (int)rh.TotalMinutes;
            return copy;
        }

        /// <summary>
        ///     Calculate the difference in time between two <see cref="EorzeaDateTime" />.
        /// </summary>
        /// <param name="lh"><see cref="EorzeaDateTime" /> to use as source.</param>
        /// <param name="rh"><see cref="EorzeaDateTime" /> to use as destination.</param>
        /// <returns>Returns the <see cref="TimeSpan" /> from <see cref="lh" /> to <see cref="rh" />.</returns>
        public static TimeSpan operator-(EorzeaDateTime lh, EorzeaDateTime rh) {
            return lh.GetTimeSpan() - rh.GetTimeSpan();
        }

        /// <summary>
        ///     Check if a <see cref="EorzeaDateTime" /> is before or equal to another <see cref="EorzeaDateTime" />.
        /// </summary>
        /// <param name="lh"><see cref="EorzeaDateTime" /> to check.</param>
        /// <param name="rh"><see cref="EorzeaDateTime" /> to check against.</param>
        /// <returns>Returns <c>true</c> if <see cref="lh" /> is before or equal to <see cref="rh" />; <c>false</c> otherwise.</returns>
        public static bool operator<=(EorzeaDateTime lh, EorzeaDateTime rh) {
            return lh.GetUnixTime() <= rh.GetUnixTime();
        }

        /// <summary>
        ///     Check if a <see cref="EorzeaDateTime" /> is before another <see cref="EorzeaDateTime" />.
        /// </summary>
        /// <param name="lh"><see cref="EorzeaDateTime" /> to check.</param>
        /// <param name="rh"><see cref="EorzeaDateTime" /> to check against.</param>
        /// <returns>Returns <c>true</c> if <see cref="lh" /> is before <see cref="rh" />; <c>false</c> otherwise.</returns>
        public static bool operator<(EorzeaDateTime lh, EorzeaDateTime rh) {
            return lh.GetUnixTime() < rh.GetUnixTime();
        }

        /// <summary>
        ///     Check if a <see cref="EorzeaDateTime" /> is after or equal to another <see cref="EorzeaDateTime" />.
        /// </summary>
        /// <param name="lh"><see cref="EorzeaDateTime" /> to check.</param>
        /// <param name="rh"><see cref="EorzeaDateTime" /> to check against.</param>
        /// <returns>Returns <c>true</c> if <see cref="lh" /> is after or equal to <see cref="rh" />; <c>false</c> otherwise.</returns>
        public static bool operator>=(EorzeaDateTime lh, EorzeaDateTime rh) {
            return lh.GetUnixTime() >= rh.GetUnixTime();
        }

        /// <summary>
        ///     Check if a <see cref="EorzeaDateTime" /> is after another <see cref="EorzeaDateTime" />.
        /// </summary>
        /// <param name="lh"><see cref="EorzeaDateTime" /> to check.</param>
        /// <param name="rh"><see cref="EorzeaDateTime" /> to check against.</param>
        /// <returns>Returns <c>true</c> if <see cref="lh" /> is after <see cref="rh" />; <c>false</c> otherwise.</returns>
        public static bool operator>(EorzeaDateTime lh, EorzeaDateTime rh) {
            return lh.GetUnixTime() > rh.GetUnixTime();
        }

        /// <summary>
        ///     Clones the value of this <see cref="EorzeaDateTime" /> into a new <see cref="EorzeaDateTime" />.
        /// </summary>
        /// <returns>Returns a new <seealso cref="EorzeaDateTime" /> object with the same value as the current.</returns>
        public EorzeaDateTime Clone() {
            return new EorzeaDateTime(this);
        }

    }
}
