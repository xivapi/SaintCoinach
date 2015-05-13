using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    public static class VectorConverter {
        public static SharpDX.Vector3 ToDx(this Graphics.Vector3 self) {
            return new SharpDX.Vector3 {
                X = self.X,
                Y = self.Y,
                Z = self.Z
            };
        }
        public static SharpDX.Vector3 ToDx(this Graphics.Vector3? self, SharpDX.Vector3 defaultValue) {
            if (self.HasValue)
                return self.Value.ToDx();
            return defaultValue;
        }
        public static SharpDX.Vector4 ToDx(this Graphics.Vector4 self) {
            return new SharpDX.Vector4 {
                X = self.X,
                Y = self.Y,
                Z = self.Z,
                W = self.W
            };
        }
        public static SharpDX.Vector3 ToDx3(this Graphics.Vector4 self) {
            return new SharpDX.Vector3 {
                X = self.X,
                Y = self.Y,
                Z = self.Z
            };
        }
        public static SharpDX.Vector4 ToDx(this Graphics.Vector4? self, SharpDX.Vector4 defaultValue) {
            if (self.HasValue)
                return self.Value.ToDx();
            return defaultValue;
        }
    }
}
