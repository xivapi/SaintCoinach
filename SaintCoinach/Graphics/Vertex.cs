using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public struct Vertex {
        public Vector4? Position;
        public Vector4? BlendWeights;
        public uint? BlendIndices;
        public Vector3? Normal;

        // TODO: For dual textures it's two components per texture, unknown purpose of Z and W components on others.
        public Vector4? UV;
        public Vector4? Color;
        public Vector4? Tangent2;
        public Vector4? Tangent1;
    }
}
