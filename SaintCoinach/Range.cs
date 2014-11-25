using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach {
    /// <summary>
    /// Helper structure to denote mark ranges.
    /// </summary>
    public struct Range {
        public static readonly Range Zero = new Range(0, 0);

        public readonly int Start;
        public readonly int Length;
        public int End { get { return Start + Length; } }

        public Range(int start, int length) {
            Start = start;
            Length = length;
        }

        public bool Contains(int value) {
            return value >= Start && value < End;
        }

        public bool Intersects(Range other) {
            return this.Contains(other.Start)
                || this.Contains(other.End - 1)
                || other.Contains(this.Start)
                || other.Contains(this.End - 1);
        }

        public static bool Contains(IEnumerable<Range> ranges, int value) {
            return ranges.Any(_ => _.Contains(value));
        }

        /// <summary>
        /// Optimizes multiple ranges into the minimum number of ranges
        /// required to represent the same values.
        /// </summary>
        /// <param name="ranges">The ranges to optimize.</param>
        /// <returns>An array of the optimized ranges.</returns>
        public static Range[] Combine(IEnumerable<Range> ranges) {
            if (!ranges.Any())
                return new Range[0];

            var combined = new List<Range>();

            var ordered = ranges.OrderBy(_ => _.Start);
            var currentStart = ordered.First().Start;
            var currentEnd = currentStart;
            foreach (var range in ordered) {
                if (range.Start > currentEnd) {
                    if (currentEnd > currentStart)
                        combined.Add(new Range(currentStart, currentEnd - currentStart));

                    currentStart = range.Start;
                    currentEnd = range.End;
                } else
                    currentEnd = Math.Max(currentEnd, range.End);
            }
            if (currentEnd > currentStart)
                combined.Add(new Range(currentStart, currentEnd - currentStart));

            return combined.OrderBy(_ => _.Start).ToArray();
        }
    }
}
