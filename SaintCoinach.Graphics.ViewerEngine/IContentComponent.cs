using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public interface IContentComponent : IComponent {
        bool IsLoaded { get; }

        void Load(SharpDX.Direct3D11.Device device);
        void Unload();
    }
}
