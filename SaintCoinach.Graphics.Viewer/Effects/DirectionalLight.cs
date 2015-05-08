using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Effects {
    using SharpDX;

    [StructLayout(LayoutKind.Sequential)]
    public struct DirectionalLight {
        public Vector3 Direction;
        public Vector4 Diffuse;
        public Vector4 Specular;
    }
}
