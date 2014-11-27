using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {    
    using DX = SharpDX;
    using SC = Assets;

    public static class VectorExtensions {
        public static DX.Vector2 ToDX(this SC.Vector2 self) {
            return new DX.Vector2(self.X, self.Y);
        }
        public static DX.Vector3 ToDX(this SC.Vector3 self) {
            return new DX.Vector3(self.X, self.Y, self.Z);
        }
        public static DX.Vector4 ToDX(this SC.Vector4 self) {
            return new DX.Vector4(self.X, self.Y, self.Z, self.W);
        }
    }
}
