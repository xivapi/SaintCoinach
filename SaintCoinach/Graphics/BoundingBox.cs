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
    }
}
