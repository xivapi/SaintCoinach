using System;

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

            var s1 = v1 as string;
            var s2 = v2 as string;

            if (s1 == null || s2 == null) return false;

            var maxDistance = Math.Ceiling(RelativeLevenshteinDistance * (s1.Length + s2.Length) / 2.0);
            var d = Levenshtein.Compute(s1, s2);
            return (d <= maxDistance);
        }

        private static bool IsPrimitive(object o) {
            return (o is Boolean
                    || o is Byte
                    || o is SByte
                    || o is Int16
                    || o is Int32
                    || o is Int64
                    || o is UInt16
                    || o is UInt32
                    || o is UInt64
                    || o is Single
                    || o is Double);
        }

        private static Decimal ToDecimal(object o) {
            return Convert.ToDecimal(o);
        }
    }
}
