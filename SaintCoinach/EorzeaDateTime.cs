using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaintCoinach {
    public class EorzeaDateTime {
        public static readonly DateTime Zero = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public const double RealToEorzeanFactor = 60.0 / (35 / 12.0);

        private int _Minute;
        private int _Bell;
        private int _Sun;
        private int _Moon;
        private int _Year;

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
        public int Year {
            get { return _Year; }
            set {
                _Year = value;
            }
        }

        public double TotalMinutes {
            get { return Minute + (60.0 * (Bell + 24.0 * (Sun + 32.0 * (Moon + 12.0 * Year)))); }
        }
        public double TotalBells {
            get { return Bell + (Minute / 60.0) + (24 * (Sun + 32.0 * (Moon + 12.0 * Year))); }
        }
        public double TotalSuns {
            get { return Sun + ((Bell + (Minute / 60.0)) / 24.0) + 32 * (Moon + 12.0 * Year); }
        }
        public double TotalMoons {
            get { return Moon + ((Sun + (Bell + (Minute / 60.0)) / 24.0) / 32.0) + 12.0 * Year; }
        }
        public double TotalYears {
            get { return Year + ((Moon + ((Sun + (Bell + (Minute / 60.0)) / 24.0) / 32.0)) / 12.0); }
        }

        public EorzeaDateTime() : this(DateTime.UtcNow) { }
        public EorzeaDateTime(int unixTime) : this((long)unixTime) { }
        public EorzeaDateTime(long unixTime) {
            SetUnixTime(unixTime);
        }
        public EorzeaDateTime(DateTime time) {
            SetRealTime(time);
        }

        public TimeSpan GetTimeSpan() {
            return new TimeSpan(Sun + (Year * 12 + Moon) * 32, Bell, Minute, 0, 0);
        }
        public long GetUnixTime() {
            var years = Year;
            var moons = years * 12.0 + Moon - 1;
            var suns = moons * 32.0 + Sun - 1;
            var bells = suns * 24.0 + Bell;
            var minutes = bells * 60.0 + Minute;
            var seconds = minutes * 60.0;

            return (long)(seconds / RealToEorzeanFactor);
        }
        public EorzeaDateTime SetUnixTime(long time) {
            var eorzeaSeconds = time * RealToEorzeanFactor;

            SetEorzeaTime(eorzeaSeconds);

            return this;
        }
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
        public DateTime GetRealTime() {
            return (Zero + TimeSpan.FromSeconds(GetUnixTime()));
        }
        public EorzeaDateTime SetRealTime(DateTime time) {
            var fromZero = time - Zero;
            return SetUnixTime((long)fromZero.TotalSeconds);
        }

        public override string ToString() {
            return string.Format("{0:D2}/{1:D2}/{2} {3:D2}:{4:D2}", this.Sun, this.Moon, this.Year, this.Bell, this.Minute);
        }

        public static EorzeaDateTime operator +(EorzeaDateTime lh, TimeSpan rh) {
            var copy = lh.Clone();
            copy.Minute += (int)rh.TotalMinutes;
            return copy;
        }
        public static EorzeaDateTime operator -(EorzeaDateTime lh, TimeSpan rh) {
            var copy = lh.Clone();
            copy.Minute -= (int)rh.TotalMinutes;
            return copy;
        }

        public static TimeSpan operator -(EorzeaDateTime lh, EorzeaDateTime rh) {
            return (lh.GetTimeSpan() - rh.GetTimeSpan());
        }

        public static bool operator <=(EorzeaDateTime lh, EorzeaDateTime rh) {
            return lh.GetUnixTime() <= rh.GetUnixTime();
        }
        public static bool operator <(EorzeaDateTime lh, EorzeaDateTime rh) {
            return lh.GetUnixTime() < rh.GetUnixTime();
        }
        public static bool operator >=(EorzeaDateTime lh, EorzeaDateTime rh) {
            return lh.GetUnixTime() >= rh.GetUnixTime();
        }
        public static bool operator >(EorzeaDateTime lh, EorzeaDateTime rh) {
            return lh.GetUnixTime() > rh.GetUnixTime();
        }

        public EorzeaDateTime Clone() {
            return new EorzeaDateTime() {
                Minute = this.Minute,
                Bell = this.Bell,
                Sun = this.Sun,
                Moon = this.Moon,
                Year = this.Year
            };
        }
    }
}
