using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using Device = SharpDX.Direct3D11.Device;

namespace SaintCoinach.Graphics.Viewer {
    public interface IDrawable3DComponent : IComponent {
        bool IsVisible { get; }
        /// <summary>
        /// While the arguments are refs, do not change them, it's just so they don't get cloned again and again and again.
        /// </summary>
        void Draw(EngineTime time, ref Matrix world, ref Matrix view, ref Matrix projection);
    }
}
