using System;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace SaintCoinach.Graphics.Viewer.Primitves {
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionNormalTexture {
        public Vector4 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinates;
    }
}
