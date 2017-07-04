using SaintCoinach.Xiv;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update {
    public abstract class ColumnComparer {
        private bool[] _compatibleColumnsByIndex;

        public static ColumnComparer Create(Column column, IEnumerable<Column> candidates) {
            ColumnComparer comparer;
            var type = column.Reader.Type;

            if (Comparer.IsPrimitiveType(type) || type == typeof(Quad))
                comparer = new PrimitiveColumnComparer();
            else if (type == typeof(Text.XivString) || type == typeof(string))
                comparer = new StringColumnComparer();
            else
                return null;

            comparer._compatibleColumnsByIndex = candidates.Select(c => comparer.IsCompatibleType(type, c.Reader.Type)).ToArray();
            return comparer;
        }

        public bool IsCompatibleIndex(int index) {
            return _compatibleColumnsByIndex[index];
        }

        public abstract bool IsCompatibleType(Type type, Type candidateType);

        public abstract bool Compare(object v1, object v2);
    }

    public class PrimitiveColumnComparer : ColumnComparer {
        public override bool Compare(object v1, object v2) {
            return Equals(v1, v2);
        }

        public override bool IsCompatibleType(Type type, Type candidateType) {
            return type == candidateType;
        }
    }

    public class StringColumnComparer : ColumnComparer {
        public const double RelativeLevenshteinDistance = 0.05;

        public override bool Compare(object v1, object v2) {
            string s1 = null, s2 = null;

            if (v1 is Text.XivString)
                s1 = ((Text.XivString)v1).ToString();
            else if (v1 is string)
                s1 = (string)v1;

            if (v2 is Text.XivString)
                s2 = ((Text.XivString)v2).ToString();
            else if (v2 is string)
                s2 = (string)v2;

            if (s1 == null || s2 == null) return false;

            var maxDistance = Math.Ceiling(RelativeLevenshteinDistance * (s1.Length + s2.Length) / 2.0);
            var d = ComputeLevenshtein(s1, s2);
            return (d <= maxDistance);
        }

        public static int ComputeLevenshtein(string s, string t)  {
            if (s == t) {
                // Optimization: Quick test of identical strings. 
                return 0;
            }

            var n = s.Length;
            var m = t.Length;
            var d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0) {
                return m;
            }

            if (m == 0) {
                return n;
            }

            // Step 2
            for (var i = 0; i <= n; d[i, 0] = i++) { }

            for (var j = 0; j <= m; d[0, j] = j++) { }

            // Step 3
            for (var i = 1; i <= n; i++) {
                //Step 4
                for (var j = 1; j <= m; j++) {
                    // Step 5
                    var cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                                       Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        public override bool IsCompatibleType(Type type, Type candidateType) {
            return candidateType == typeof(Text.XivString) || candidateType == typeof(string);
        }
    }
}
