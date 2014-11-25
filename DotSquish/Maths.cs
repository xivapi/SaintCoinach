using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotSquish {
    internal struct Vector3 {
        public float X, Y, Z;

        public Vector3(float value) {
            this.X = this.Y = this.Z = value;
        }
        public Vector3(float x, float y, float z) {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2) {
            return new Vector3(
                v1.X + v2.X,
                v1.Y + v2.Y,
                v1.Z + v2.Z);
        }
        public static Vector3 operator -(Vector3 v) {
            return new Vector3(
                - v.X,
                - v.Y,
                - v.Z);
        }
        public static Vector3 operator -(Vector3 v1, Vector3 v2) {
            return new Vector3(
                v1.X - v2.X,
                v1.Y - v2.Y,
                v1.Z - v2.Z);
        }
        public static Vector3 operator *(Vector3 v1, Vector3 v2) {
            return new Vector3(
                v1.X * v2.X,
                v1.Y * v2.Y,
                v1.Z * v2.Z);
        }
        public static Vector3 operator *(Vector3 v1, float v2) {
            return new Vector3(
                v1.X * v2,
                v1.Y * v2,
                v1.Z * v2);
        }
        public static Vector3 operator *(float v1, Vector3 v2) {
            return (v2 * v1);
        }
        public static Vector3 operator /(Vector3 v1, Vector3 v2) {
            return new Vector3(
                v1.X / v2.X,
                v1.Y / v2.Y,
                v1.Z / v2.Z);
        }
        public static Vector3 operator /(Vector3 v1, float v2) {
            return new Vector3(
                v1.X / v2,
                v1.Y / v2,
                v1.Z / v2);
        }

        public static float Dot(Vector3 v1, Vector3 v2) {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }
        public static Vector3 Min(Vector3 v1, Vector3 v2) {
            return new Vector3(
                (float)Math.Min(v1.X, v2.X),
                (float)Math.Min(v1.Y, v2.Y),
                (float)Math.Min(v1.Z, v2.Z));
        }
        public static Vector3 Max(Vector3 v1, Vector3 v2) {
            return new Vector3(
                (float)Math.Max(v1.X, v2.X),
                (float)Math.Max(v1.Y, v2.Y),
                (float)Math.Max(v1.Z, v2.Z));
        }
        public static Vector3 Max(Vector3 v) {
            return new Vector3(
                (float)(v.X > 0f ? Math.Floor(v.X) : Math.Ceiling(v.X)),
                (float)(v.Y > 0f ? Math.Floor(v.Y) : Math.Ceiling(v.Y)),
                (float)(v.Z > 0f ? Math.Floor(v.Z) : Math.Ceiling(v.Z)));
        }
    }
    internal struct Vector4 {
        public float X, Y, Z, W;

        public Vector4(float value) {
            this.X = this.Y = this.Z = this.W = value;
        }
        public Vector4(float x, float y, float z, float w) {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public static Vector4 operator +(Vector4 v1, Vector4 v2) {
            return new Vector4(
                v1.X + v2.X,
                v1.Y + v2.Y,
                v1.Z + v2.Z,
                v1.W + v2.W);
        }
        public static Vector4 operator -(Vector4 v) {
            return new Vector4(
                -v.X,
                -v.Y,
                -v.Z,
                -v.W);
        }
        public static Vector4 operator -(Vector4 v1, Vector4 v2) {
            return new Vector4(
                v1.X - v2.X,
                v1.Y - v2.Y,
                v1.Z - v2.Z,
                v1.W - v2.W);
        }
        public static Vector4 operator *(Vector4 v1, Vector4 v2) {
            return new Vector4(
                v1.X * v2.X,
                v1.Y * v2.Y,
                v1.Z * v2.Z,
                v1.W * v2.W);
        }
        public static Vector4 operator *(Vector4 v1, float v2) {
            return new Vector4(
                v1.X * v2,
                v1.Y * v2,
                v1.Z * v2,
                v1.W * v2);
        }
        public static Vector4 operator *(float v1, Vector4 v2) {
            return (v2 * v1);
        }
        public static Vector4 operator /(Vector4 v1, Vector4 v2) {
            return new Vector4(
                v1.X / v2.X,
                v1.Y / v2.Y,
                v1.Z / v2.Z,
                v1.W / v2.W);
        }
        public static Vector4 operator /(Vector4 v1, float v2) {
            return new Vector4(
                v1.X / v2,
                v1.Y / v2,
                v1.Z / v2,
                v1.W / v2);
        }
    }

    internal class Sym3x3 {
        private float[] _X = new float[6];
        public float this[int index] {
            get { return this._X[index]; }
            set { this._X[index] = value; }
        }

        public Sym3x3() { }
        public Sym3x3(float s) {
            for (int i = 0; i < 6; ++i)
                this._X[i] = s;
        }

        public static Sym3x3 ComputeWeightedCovariance(int n, Vector3[] points, float[] weights) {
            // Compute the centroid.
            var total = 0f;
            var centroid = new Vector3(0f);
            for (int i = 0; i < n; ++i) {
                total += weights[i];
                centroid += weights[i] * points[i];
            }
            centroid /= total;

            // Accumulate the covariance matrix.
            var covariance = new Sym3x3(0f);
            for (int i = 0; i < n; ++i) {
                var a = points[i] - centroid;
                var b = weights[i] * a;

                covariance[0] += a.X * b.X;
                covariance[1] += a.X * b.Y;
                covariance[2] += a.X * b.Z;
                covariance[3] += a.Y * b.Y;
                covariance[4] += a.Y * b.Z;
                covariance[5] += a.Z * b.Z;
            }

            return covariance;
        }
        private static Vector3 GetMultiplicity1Evector(Sym3x3 matrix, float evalue) {
            // Compute M
            var m = new Sym3x3();
            m[0] = matrix[0] - evalue;
            m[1] = matrix[1];
            m[2] = matrix[2];
            m[3] = matrix[3] - evalue;
            m[4] = matrix[4];
            m[5] = matrix[5] - evalue;

            // Compute U
            var u = new Sym3x3();
            u[0] = (m[3] * m[5]) - (m[4] * m[4]);
            u[1] = (m[2] * m[4]) - (m[1] * m[5]);
            u[2] = (m[1] * m[4]) - (m[2] * m[3]);
            u[3] = (m[0] * m[5]) - (m[2] * m[2]);
            u[4] = (m[1] * m[2]) - (m[4] * m[0]);
            u[5] = (m[0] * m[3]) - (m[1] * m[1]);

            // Find the largest component.
            var mc = Math.Abs(u[0]);
            var mi = 0;
            for (int i = 1; i < 6; ++i) {
                var c = Math.Abs(u[i]);
                if (c > mc) {
                    mc = c;
                    mi = i;
                }
            }

            // Pick the column with this component.
            switch (mi) {
                case 0:
                    return new Vector3(u[0], u[1], u[2]);
                case 1:
                case 3:
                    return new Vector3(u[1], u[3], u[4]);
                default:
                    return new Vector3(u[2], u[4], u[5]);
            }
        }
        private static Vector3 GetMultiplicity2Evector(Sym3x3 matrix, float evalue) {
            // Compute M
            var m = new Sym3x3();
            m[0] = matrix[0] - evalue;
            m[1] = matrix[1];
            m[2] = matrix[2];
            m[3] = matrix[3] - evalue;
            m[4] = matrix[4];
            m[5] = matrix[5] - evalue;

            // Find the largest component.
            var mc = Math.Abs(m[0]);
            var mi = 0;
            for (int i = 1; i < 6; ++i) {
                var c = Math.Abs(m[i]);
                if (c > mc) {
                    mc = c;
                    mi = i;
                }
            }

            // pick the first eigenvector based on this index
            switch (mi) {
                case 0:
                case 1:
                    return new Vector3(-m[1], m[0], 0.0f);

                case 2:
                    return new Vector3(m[2], 0.0f, -m[0]);

                case 3:
                case 4:
                    return new Vector3(0.0f, -m[4], m[3]);

                default:
                    return new Vector3(0.0f, -m[5], m[4]);
            }
        }
        public static Vector3 ComputePrincipledComponent(Sym3x3 matrix) {
            // Compute the cubic coefficients
            var c0 =
                (matrix[0] * matrix[3] * matrix[5])
                + (matrix[1] * matrix[2] * matrix[4] * 2f)
                - (matrix[0] * matrix[4] * matrix[4])
                - (matrix[3] * matrix[2] * matrix[2])
                - (matrix[5] * matrix[1] * matrix[1]);
            var c1 =
                (matrix[0] * matrix[3])
                + (matrix[0] * matrix[5])
                + (matrix[3] * matrix[5])
                - (matrix[1] * matrix[1])
                - (matrix[2] * matrix[2])
                - (matrix[4] * matrix[4]);
            var c2 = matrix[0] + matrix[3] + matrix[5];

            // Compute the quadratic coefficients
            var a = c1 - ((1f / 3f) * c2 * c2);
            var b = ((-2f / 27f) * c2 * c2 * c2) + ((1f / 3f) * c1 * c2) - c0;

            // Compute the root count check;
            var Q = (.25f * b * b) + ((1f / 27f) * a * a * a);

            // Test the multiplicity.
            if (float.Epsilon < Q)
                return new Vector3(1f); // Only one root, which implies we have a multiple of the identity.
            else if (Q < -float.Epsilon) {
                // Three distinct roots
                var theta = Math.Atan2(Math.Sqrt(Q), -.5f * b);
                var rho = Math.Sqrt((.25f * b * b) - Q);

                var rt = Math.Pow(rho, 1f / 3f);
                var ct = Math.Cos(theta / 3f);
                var st = Math.Sin(theta / 3f);

                var l1 = ((1f / 3f) * c2) + (2f * rt * ct);
                var l2 = ((1f / 3f) * c2) - (rt * (ct + (Math.Sqrt(3f) * st)));
                var l3 = ((1f / 3f) * c2) - (rt * (ct - (Math.Sqrt(3f) * st)));

                // Pick the larger.
                if (Math.Abs(l2) > Math.Abs(l1))
                    l1 = l2;
                if (Math.Abs(l3) > Math.Abs(l1))
                    l1 = l3;

                // Get the eigenvector
                return GetMultiplicity1Evector(matrix, (float)l1);
            } else {    // Q very close to 0
                // Two roots
                double rt;
                if (b < 0.0f)
                    rt = -Math.Pow(-.5f * b, 1f / 3f);
                else
                    rt = Math.Pow(.5f * b, 1f / 3f);

                var l1 = ((1f / 3f) * c2) + rt;
                var l2 = ((1f / 3f) * c2) - (2f * rt);

                // Get the eigenvector
                if (Math.Abs(l1) > Math.Abs(l2))
                    return GetMultiplicity2Evector(matrix, (float)l1);
                else
                    return GetMultiplicity1Evector(matrix, (float)l2);
            }
        }
    }
}
