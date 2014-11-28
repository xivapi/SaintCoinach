using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using Device = SharpDX.Direct3D11.Device;

namespace SaintCoinach.Graphics {
    public interface IDrawable : IComponent {
        void Draw(Device device, EngineTime time, ref Matrix world, ref Matrix view, ref Matrix projection);
    }
}
