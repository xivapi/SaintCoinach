using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    public interface IUpdateableComponent : IComponent {
        bool IsEnabled { get; }
        void Update(EngineTime engineTime);
    }
}
