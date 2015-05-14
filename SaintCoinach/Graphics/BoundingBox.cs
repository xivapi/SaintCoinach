using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [StructLayout(LayoutKind.Sequential)]
    public struct BoundingBox {
        public Vector4 PointA;
        public Vector4 PointB;

        public BoundingBox Scale(float factor) { return Scale(new Vector3 { X = factor, Y = factor, Z = factor }); }
        public BoundingBox Scale(Vector3 factor) {
            var center = new Vector3 {
                X = (PointA.X + PointB.X) / 2f,
                Y = (PointA.Y + PointB.Y) / 2f,
                Z = (PointA.Z + PointB.Z) / 2f
            };
            var d = new Vector3 {
                X = Math.Abs(PointA.X - center.X) * factor.X,
                Y = Math.Abs(PointA.Y - center.Y) * factor.Y,
                Z = Math.Abs(PointA.Z - center.Z) * factor.Z
            };

            var retPointA = new Vector4 {
                X = center.X + d.X,
                Y = center.Y + d.Y,
                Z = center.Z + d.Z,
                W = PointA.W
            };
            var retPointB = new Vector4 {
                X = center.X - d.X,
                Y = center.Y - d.Y,
                Z = center.Z - d.Z,
                W = PointB.W
            };

            return new BoundingBox {
                PointA = retPointA,
                PointB = retPointB
            };
        }
        public BoundingBox Grow(float change) { return Grow(new Vector3 { X = change, Y = change, Z = change }); }
        public BoundingBox Grow(Vector3 change) {
            var center = new Vector3 {
                X = (PointA.X + PointB.X) / 2f,
                Y = (PointA.Y + PointB.Y) / 2f,
                Z = (PointA.Z + PointB.Z) / 2f
            };
            var d = new Vector3 {
                X = Math.Abs(PointA.X - center.X) + change.X,
                Y = Math.Abs(PointA.Y - center.Y) + change.Y,
                Z = Math.Abs(PointA.Z - center.Z) + change.Z
            };

            var retPointA = new Vector4 {
                X = center.X + d.X,
                Y = center.Y + d.Y,
                Z = center.Z + d.Z,
                W = PointA.W
            };
            var retPointB = new Vector4 {
                X = center.X - d.X,
                Y = center.Y - d.Y,
                Z = center.Z - d.Z,
                W = PointB.W
            };

            return new BoundingBox {
                PointA = retPointA,
                PointB = retPointB
            };
        }
    }
}
