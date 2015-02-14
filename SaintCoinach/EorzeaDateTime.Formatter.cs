using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach {
    public partial class EorzeaDateTime : IFormattable {

        #region IFormattable Members

        /// <summary>
        ///     Get the string representation of this <see cref="EorzeaDateTime" /> using the default format string.
        /// </summary>
        /// <remarks>
        ///     The format of the returned string is <c>Sun/Moon/Year Bell:Minute</c>, with all but Year having a leading zero of
        ///     less than 10.
        /// </remarks>
        /// <returns>Returns the string representation of this <see cref="EorzeaDateTime" />.</returns>
        public override string ToString() {
            return this.ToString(null, System.Globalization.CultureInfo.CurrentCulture);
            //return string.Format("{0:D2}/{1:D2}/{2} {3:D2}:{4:D2}", Sun, Moon, Year, Bell, Minute);
        }

        /// <summary>
        ///    	Formats the value of the current instance using the specified format, or the default format if none is provided.
        /// </summary>
        /// <remarks>
        ///     Available standard formats are:
        ///       G     Equivalent to 'dd/MM/y HH:mm'
        ///       g     Equivalent to 'dd/MM/y hh:mm p'
        ///       T     Equivalent to 'HH:mm'
        ///       t     Equivalent to 'hh:mm p'
        ///       
        ///     Available custom specifiers are:
        ///       d     The day of the month, from 1 through 32.
        ///       dd    The day of the month, from 01 through 32.
        ///       h     The hour, using a 12-hour clock from 1 through 12.
        ///       hh    The hour, using a 12-hour clock from 01 through 12.
        ///       H     The hour, using a 24-hour clock from 0 through 23.
        ///       HH    The hour, using a 24-hour clock from 00 through 23.
        ///       m     The minute, from 0 through 59.
        ///       mm    The minute, from 00 through 59.
        ///       M     The month, from 1 through 12.
        ///       MM    The month, from 01 through 12.
        ///       p     The AM/PM designator.
        ///       y     The year.
        ///       yyy   The year as a three-digit number.
        ///       yyyy  The year as a four-digit number.
        /// </remarks>
        /// <param name="format">The format to use.</param>
        /// <param name="formatProvider">The provider to use to format the value.</param>
        /// <returns>The value of the current instance in the specified format.</returns>
        public string ToString(string format, IFormatProvider formatProvider) {
            return Formatter.Format(this, format, formatProvider);
        }

        #endregion

        /// <summary>
        ///     Static class for formatting instances of the <see cref="EorzeaDateTime"/> class to strings.
        /// </summary>
        private static class Formatter {
            /// <summary>
            ///     The default format.
            /// </summary>
            public const string DefaultFormat = "G";

            /// <summary>
            ///     Mapping of characters to their format string.
            /// </summary>
            /// <remarks>
            ///     The key character must be the first, and only, character in the original format string.
            /// </remarks>
            private static readonly Dictionary<char, string> StandardFormats = new Dictionary<char, string> {
                { 'G', "dd/MM/y HH:mm" },
                { 'g', "dd/MM/y hh:mm p" },
                { 'T', "HH:mm" },
                { 't', "hh:mm p" },
            };

            /// <summary>
            ///     Mapping of valid specifiers.
            /// </summary>
            private static readonly Dictionary<char, Specifier> KnownSpecifiers = new Dictionary<char, Specifier> {
                { 'd', new Specifier {
                     MaximumCount = 2,
                     FormatFunction = (dt, c, p) => dt.Sun.ToString("D" + c.ToString(), p)
                } },
                { 'M', new Specifier {
                     MaximumCount = 2,
                     FormatFunction = (dt, c, p) => dt.Moon.ToString("D" + c.ToString(), p)
                } },
                { 'y', new Specifier {
                     MaximumCount = 4,
                     FormatFunction = (dt, c, p) => dt.Year.ToString("D" + c.ToString(), p)
                } },
                { 'H', new Specifier {
                     MaximumCount = 2,
                     FormatFunction = (dt, c, p) => dt.Bell.ToString("D" + c.ToString(), p)
                } },
                { 'h', new Specifier {
                     MaximumCount = 2,
                     FormatFunction = (dt, c, p) => {
                         var t = dt.Bell % 12;
                         if(t == 0)
                             t = 12;
                         return t.ToString("D" + c.ToString(), p);
                     }
                } },
                { 'm', new Specifier {
                     MaximumCount = 2,
                     FormatFunction = (dt, c, p) => dt.Minute.ToString("D" + c.ToString(), p)
                } },
                
                { 'p', new Specifier {
                     MaximumCount = 2,
                     FormatFunction = (dt, c, p) => {
                         var dtInfo = System.Globalization.DateTimeFormatInfo.GetInstance(p);
                         return dt.Bell < 12 ? dtInfo.AMDesignator : dtInfo.PMDesignator;
                     }
                } },
            };
            /// <summary>
            ///     Array of characters that can be used to encapsulate string literals.
            /// </summary>
            private static readonly char[] StringSpecifiers = new char[] { '\'', '"' };

            /// <summary>
            ///    	Formats the value of the current instance using the specified format and format provider..
            /// </summary>
            /// <param name="dateTime"><see cref="EorzeaDateTime"/> to format.</param>
            /// <param name="format">Format string to use, or <c>null</c> to use the default format.</param>
            /// <param name="formatProvider">The provider to use to format the value, or <c>null</c> if the current culture's provider should be used.</param>
            /// <returns>The value of <c>dateTime</c> formatted as a string according to <c>format</c>.</returns>
            public static string Format(EorzeaDateTime dateTime, string format, IFormatProvider formatProvider) {
                if (dateTime == null)
                    throw new ArgumentNullException("dateTime");
                if (format == null)
                    format = DefaultFormat;
                if (formatProvider == null)
                    formatProvider = System.Globalization.CultureInfo.CurrentCulture;

                if (string.IsNullOrWhiteSpace(format))
                    throw new ArgumentException();

                if (format.Length == 1) {
                    var standardKey = format[0];
                    if (StandardFormats.ContainsKey(standardKey))
                        format = StandardFormats[standardKey];
                }

                var sb = new StringBuilder();

                char isInString = '\0';
                char currentSpecifier = '\0';
                var specifierCount = 0;
                foreach (var c in format) {
                    if (isInString != '\0') {
                        if (c == isInString)
                            isInString = '\0';
                        else
                            sb.Append(c);
                    } else {
                        if (c == currentSpecifier)
                            ++specifierCount;
                        else {
                            TryFinishSequence(dateTime, sb, ref currentSpecifier, specifierCount, formatProvider);

                            if (StringSpecifiers.Contains(c))
                                isInString = c;
                            else if (KnownSpecifiers.ContainsKey(c)) {
                                currentSpecifier = c;
                                specifierCount = 1;
                            } else
                                sb.Append(c);
                        }

                        if (currentSpecifier != '\0') {
                            if (specifierCount == KnownSpecifiers[currentSpecifier].MaximumCount)
                                TryFinishSequence(dateTime, sb, ref currentSpecifier, specifierCount, formatProvider);
                        }
                    }
                }
                TryFinishSequence(dateTime, sb, ref currentSpecifier, specifierCount, formatProvider);

                return sb.ToString();
            }

            /// <summary>
            ///     Append the format for a specifier to the output, if applicable.
            /// </summary>
            /// <param name="dateTime">The Source <see cref="EorzeaDateTime"/>.</param>
            /// <param name="stringBuilder"><see cref="StringBuilder"/> to write to.</param>
            /// <param name="specifier">Specifier to use for formatting, or \0 should nothing be appended. When this method returns this value will always be \0.</param>
            /// <param name="count">Subsequent occurences of <c>specifier</c>.</param>
            /// <param name="formatProvider">The provider to use to format the value.</param>
            private static void TryFinishSequence(EorzeaDateTime dateTime, StringBuilder stringBuilder, ref char specifier, int count, IFormatProvider formatProvider) {
                if (specifier == '\0')
                    return;

                stringBuilder.Append(KnownSpecifiers[specifier].FormatFunction(dateTime, count, formatProvider));
                specifier = '\0';
            }

            /// <summary>
            ///     Helper class for format specifiers.
            /// </summary>
            private class Specifier {
                /// <summary>
                ///     Gets or sets the function used format a value using the current specifier.
                /// </summary>
                /// <value>The function used format a value using the current specifier.</value>
                public Func<EorzeaDateTime, int, IFormatProvider, string> FormatFunction { get; set; }
                /// <summary>
                ///     Gets or sets the maximum number of times the current specifier can be used in sequence.
                /// </summary>
                /// <value>The maximum number of times the current specifier can be used in sequence.</value>
                public int MaximumCount { get; set; }
            }
        }
    }
}
