using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Interop {
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    struct InteropVertex {
        public InteropVector4 Position;
        public InteropVector4 BlendWeights;
        public uint BlendIndices;
        public InteropVector4 Normal;

        public InteropVector4 UV;
        public InteropVector4 Color;
        public InteropVector4 Tangent1;
        public InteropVector4 Tangent2;
    }
}
