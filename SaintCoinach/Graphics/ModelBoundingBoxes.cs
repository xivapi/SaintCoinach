using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [StructLayout(LayoutKind.Sequential)]
    public struct ModelBoundingBoxes {  // 9 in hsl
        // TODO: Purpose of having 4 boxes (or 8 vectors) is unknown.
        public BoundingBox Value1;
        public BoundingBox Value2;
        public BoundingBox Value3;
        public BoundingBox Value4;
    }
}
