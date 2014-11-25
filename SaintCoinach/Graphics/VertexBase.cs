using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class VertexBase {
        public Vector4 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 TextureCoordinates0 { get; set; }
    }
}
