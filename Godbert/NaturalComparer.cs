// Taken from http://www.codeproject.com/Articles/22517/Natural-Sort-Comparer
// Modified to allow for different StringComparison
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Godbert {
    public class NaturalComparer : Comparer<string>, IDisposable {
        private Dictionary<string, string[]> table;

        public NaturalComparer() : this(StringComparer.Ordinal) { }
        public NaturalComparer(StringComparer partsComparer) {
            table = new Dictionary<string, string[]>();
            this.PartsComparer = partsComparer;
        }

        public StringComparer PartsComparer { get; set; }

        public void Dispose() {
            table.Clear();
            table = null;
        }

        public override int Compare(string x, string y) {
            if (x == y) {
                return 0;
            }
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            string[] x1, y1;
            if (!table.TryGetValue(x, out x1)) {
                x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
                table.Add(x, x1);
            }
            if (!table.TryGetValue(y, out y1)) {
                y1 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");
                table.Add(y, y1);
            }

            for (int i = 0; i < x1.Length && i < y1.Length; i++) {
                if (x1[i] != y1[i]) {
                    // This always returned, modified to only return if not equal
                    var partCmp = PartCompare(x1[i], y1[i]);
                    if (partCmp != 0)
                        return partCmp;
                }
            }
            if (y1.Length > x1.Length) {
                return 1;
            } else if (x1.Length > y1.Length) {
                return -1;
            } else {
                return 0;
            }
        }

        private int PartCompare(string left, string right) {
            int x, y;
            if (!int.TryParse(left, out x) || !int.TryParse(right, out y))
                return PartsComparer.Compare(left, right);

            return x.CompareTo(y);
        }
    }
}