using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace SaintCoinach.Graphics {
    [StructLayout(LayoutKind.Sequential)]
    public struct PointLight {
        public Vector3 Direction;
        public Vector3 Diffuse;
        public Vector3 Specular;
    }
}
