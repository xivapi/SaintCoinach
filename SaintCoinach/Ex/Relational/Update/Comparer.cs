using SaintCoinach.Xiv;
using System;
using System.Collections;

namespace SaintCoinach.Ex.Relational.Update {
    static partial class Comparer {
        #region Static

        private const double RelativeLevenshteinDistance = 0.05;

        #endregion

        public static bool IsMatch(object v1, object v2) {
            if (IsPrimitive(v1) && IsPrimitive(v2)) {
                if (v1.GetType() != v2.GetType()) return false;

                var d1 = ToDecimal(v1);
                var d2 = ToDecimal(v2);

                return d1 == d2;
            }

            if (v1 is Quad && v2 is Quad)
                return Equals(v1, v2);

            string s1 = null, s2 = null;

            if (v1 is Text.XivString)
                s1 = ((Text.XivString)v1).ToString();
            else if (v1 is string)
                s1 = (string)v1;

            if (v2 is Text.XivString)
                s2 = ((Text.XivString)v2).ToString();
            else if (v2 is string)
                s2 = (string)v2;

            if (v1 is IDictionary && v2 is IDictionary)
                return IsMatch((IDictionary)v1, (IDictionary)v2);

            if (s1 == null || s2 == null) return false;

            var maxDistance = Math.Ceiling(RelativeLevenshteinDistance * (s1.Length + s2.Length) / 2.0);
            var d = Levenshtein.Compute(s1, s2);
            return (d <= maxDistance);
        }

        public static bool IsMatch(IDictionary v1, IDictionary v2) {
            foreach (var key in v1.Keys) {
                if (!v2.Contains(key))
                    continue;

                if (!IsMatch(v1[key], v2[key]))
                    return false;
            }

            return true;
        }

        private static bool IsPrimitive(object o) {
            // The ordering of these checks is optimized based on type
            // occurrence statistics.
            if (o is UInt32) return true;
            if (o is byte) return true;
            if (o is bool) return true;
            if (o is Int32) return true;
            if (o is UInt16) return true;
            // XivString
            if (o is SByte) return true;
            if (o is Int16) return true;
            // Quad
            if (o is Single) return true;
            // Int64, UInt64, double have no occurrences.
            if (o is Int64) return true;
            if (o is UInt64) return true;
            if (o is double) return true;
            return false;
        }

        private static Decimal ToDecimal(object o) {
            return Convert.ToDecimal(o);
        }
    }
}
