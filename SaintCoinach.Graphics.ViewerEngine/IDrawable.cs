using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Device = SharpDX.Direct3D11.Device;

namespace SaintCoinach.Graphics.Viewer {
    public interface IDrawable : IComponent {
        void Draw(Device device, EngineTime time);
    }
}
