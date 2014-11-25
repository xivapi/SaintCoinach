using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class VertexBlend : VertexBase {
        public Vector4 BlendWeight { get; set; }
        public Vector2 TextureCoordinates1 { get; set; }
    }
}
