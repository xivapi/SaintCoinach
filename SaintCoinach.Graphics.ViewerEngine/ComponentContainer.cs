using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    public class ComponentContainer : Collection<IComponent>,
        IComponentContainer, IDrawable, IUpdateable {


        #region IDrawable Members
        public void Draw(SharpDX.Direct3D11.Device device, EngineTime time) {
            foreach (var c in this.OfType<IDrawable>())
                c.Draw(device, time);
        }
        #endregion

        #region IUpdateable Members
        public void Update(EngineTime time) {
            foreach (var c in this.OfType<IUpdateable>())
                c.Update(time);
        }
        #endregion
    }
}
