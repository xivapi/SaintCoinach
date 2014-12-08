using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.Practices.Prism.Mvvm;

namespace Thaliak.Services.Search {
    public abstract class IntegerRangeQuery : BindableBase, Interfaces.ISearchQuery {
        #region Fields
        private int _Minimum;
        private bool _MinimumEnabled;
        private int _Maximum;
        private bool _MaximumEnabled;
        #endregion

        #region Properties
        public int Minimum {
            get { return _Minimum; }
            set {
                _Minimum = value;
                OnPropertyChanged(() => Minimum);
            }
        }
        public bool MinimumEnabled {
            get { return _MinimumEnabled; }
            set {
                _MinimumEnabled = value;
                OnPropertyChanged(() => MinimumEnabled);
            }
        }
        public int Maximum {
            get { return _Maximum; }
            set {
                _Maximum = value;
                OnPropertyChanged(() => Maximum);
            }
        }
        public bool MaximumEnabled {
            get { return _MaximumEnabled; }
            set {
                _MaximumEnabled = value;
                OnPropertyChanged(() => MaximumEnabled);
            }
        }
        #endregion

        #region ISearchQuery Members

        public abstract IEnumerable<Type> MatchedTypes { get; }

        public abstract bool IsMatch(object value);

        private static Regex SetPattern = new Regex(
            @"^\s*(?:"
                + @"(?<Range>(?<RangeMin>\d+)?\s*-\s*(?<RangeMax>\d+)?)"
                + @"|(?<MaxOnly><\s*(?<MaxEq>=)?\s*(?<Max>\d+))"
                + @"|(?<MinOnly>>\s*(?<MinEq>=)?\s*(?<Min>\d+))"
                + @"|(?<Fixed>\d+)"
            + @")\s*$", RegexOptions.Compiled | RegexOptions.Singleline);
        public virtual void Set(string args) {
            var match = SetPattern.Match(args);
            if (match.Success) {
                if (match.Groups["Range"].Success)
                    SetRange(match);
                else if (match.Groups["MaxOnly"].Success)
                    SetMaxOnly(match);
                else if (match.Groups["MinOnly"].Success)
                    SetMinOnly(match);
                else if (match.Groups["Fixed"].Success)
                    SetFixed(match);
            } else {
                MinimumEnabled = false;
                MaximumEnabled = false;
            }
        }

        public bool IsInRange(double value) {
            if (MinimumEnabled && value < Minimum)
                return false;
            if (MaximumEnabled && value > Maximum)
                return false;
            return true;
        }
        public bool IsInRange(int value) {
            if (MinimumEnabled && value < Minimum)
                return false;
            if (MaximumEnabled && value > Maximum)
                return false;
            return true;
        }

        private void SetRange(Match match) {
            if (match.Groups["RangeMin"].Success) {
                Minimum = int.Parse(match.Groups["RangeMin"].Value);
                MinimumEnabled = true;
            } else
                MinimumEnabled = false;

            if (match.Groups["RangeMax"].Success) {
                Maximum = int.Parse(match.Groups["RangeMax"].Value);
                MaximumEnabled = true;
            } else
                MaximumEnabled = false;
        }
        private void SetMaxOnly(Match match) {
            var v = int.Parse(match.Groups["Max"].Value);
            if (!match.Groups["MaxEq"].Success)
                --v;

            Maximum = v;
            MaximumEnabled = true;
            MinimumEnabled = false;
        }
        private void SetMinOnly(Match match) {
            var v = int.Parse(match.Groups["Min"].Value);
            if (!match.Groups["MinEq"].Success)
                ++v;

            Minimum = v;
            MinimumEnabled = true;
            MaximumEnabled = false;
        }
        private void SetFixed(Match match) {
            var v = int.Parse(match.Groups["Fixed"].Value);
            Minimum = v;
            Maximum = v;
            MinimumEnabled = true;
            MaximumEnabled = true;
        }
        #endregion

        public override string ToString() {
            var sb = new StringBuilder();

            if (MinimumEnabled && MaximumEnabled) {
                sb.Append(Minimum);
                if (Maximum != Minimum)
                    sb.AppendFormat("-{0}", Maximum);
            } else if (MinimumEnabled)
                sb.AppendFormat(">={0}", Minimum);
            else if (MaximumEnabled)
                sb.AppendFormat("<={0}", Maximum);

            return sb.ToString();
        }
    }
}
