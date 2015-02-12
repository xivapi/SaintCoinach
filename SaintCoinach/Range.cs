using System;
using System.Collections.Generic;
using System.Linq;

namespace SaintCoinach {
    /// <summary>
    ///     Helper structure to denote ranges.
    /// </summary>
    public struct Range {
        #region Static

        /// <summary>
        ///     Empty range starting at 0.
        /// </summary>
        public static readonly Range Zero = new Range(0, 0);

        #endregion

        #region Fields

        /// <summary>
        ///     Length of the <see cref="Range" />.
        /// </summary>
        private readonly int _Length;

        /// <summary>
        ///     Start of the <see cref="Range" />.
        /// </summary>
        private readonly int _Start;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the end of the <see cref="Range" />.
        /// </summary>
        /// <value>End of the <see cref="Range" />.</value>
        public int End { get { return _Start + _Length; } }

        /// <summary>
        ///     Gets the length of the <see cref="Range" />.
        /// </summary>
        /// <value>Length of the <see cref="Range" />.</value>
        public int Length { get { return _Length; } }

        /// <summary>
        ///     Gets the start of the <see cref="Range" />.
        /// </summary>
        /// <value>Start of the <see cref="Range" />.</value>
        public int Start { get { return _Start; } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Range" /> struct.
        /// </summary>
        /// <param name="start">Start of the <see cref="Range" />.</param>
        /// <param name="length">Length of the <see cref="Range" />.</param>
        public Range(int start, int length) {
            _Start = start;
            _Length = length;
        }

        #endregion

        /// <summary>
        ///     Checks if a given value is contained inside this <see cref="Range" />.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Returns <c>true</c> if <c>value</c> is inside this <see cref="Range" />; <c>false</c> otherwise.</returns>
        public bool Contains(int value) {
            return value >= _Start && value < End;
        }

        /// <summary>
        ///     Checks if a given <see cref="Range" /> intersects with this <see cref="Range" />.
        /// </summary>
        /// <param name="other"><see cref="Range" /> to check.</param>
        /// <returns>Returns <c>true</c> if <c>value</c> intersects with this <see cref="Range" />; <c>false</c> otherwise.</returns>
        public bool Intersects(Range other) {
            return Contains(other._Start)
                   || Contains(other.End - 1)
                   || other.Contains(_Start)
                   || other.Contains(End - 1);
        }

        /// <summary>
        ///     Checks if a given value is contained inside any of the given <see cref="Range" />.
        /// </summary>
        /// <param name="ranges">Enumerable of <see cref="Range" />s to check against.</param>
        /// <param name="value">Value to check.</param>
        /// <returns>Returns <c>true</c> if <c>value</c> is inside any of the given <see cref="Range" />s; <c>false</c> otherwise.</returns>
        public static bool Contains(IEnumerable<Range> ranges, int value) {
            return ranges.Any(_ => _.Contains(value));
        }

        /// <summary>
        ///     Optimizes multiple ranges into the minimum number of ranges
        ///     required to represent the same values.
        /// </summary>
        /// <param name="ranges">The <see cref="Range" />s to optimize.</param>
        /// <returns>An array of the optimized ranges.</returns>
        public static Range[] Combine(IEnumerable<Range> ranges) {
            var arr = ranges.ToArray();
            if (!arr.Any())
                return new Range[0];

            var combined = new List<Range>();

            var ordered = arr.OrderBy(_ => _._Start);
            var currentStart = ordered.First()._Start;
            var currentEnd = currentStart;
            foreach (var range in ordered) {
                if (range._Start > currentEnd) {
                    if (currentEnd > currentStart)
                        combined.Add(new Range(currentStart, currentEnd - currentStart));

                    currentStart = range._Start;
                    currentEnd = range.End;
                } else
                    currentEnd = Math.Max(currentEnd, range.End);
            }
            if (currentEnd > currentStart)
                combined.Add(new Range(currentStart, currentEnd - currentStart));

            return combined.OrderBy(_ => _._Start).ToArray();
        }
    }
}
