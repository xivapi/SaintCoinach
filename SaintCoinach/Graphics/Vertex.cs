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
        public Vector4? UV;    // TODO: First two components are definitely UV, remaining two components not confirmed for anything (so far only seen values -1,2)
        public Vector4? Color;
        public Vector4? Unknown06;
    }
}
