using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace SaintCoinach.Graphics {
    public class ComponentContainer : Collection<IComponent>,
        IComponentContainer, IDrawable, IUpdateable, IContentComponent {

        #region Fields
        private bool _IsLoaded = false;
        private ViewerEngine _Engine = null;
        #endregion

        #region Collection overrides
        protected override void InsertItem(int index, IComponent item) {
            if (item is IContentComponent && IsLoaded && !((IContentComponent)item).IsLoaded)
                ((IContentComponent)item).Load(_Engine);

            base.InsertItem(index, item);
        }
        protected override void SetItem(int index, IComponent item) {
            if (item is IContentComponent && IsLoaded && !((IContentComponent)item).IsLoaded)
                ((IContentComponent)item).Load(_Engine);

            base.SetItem(index, item);
        }
        #endregion

        #region IDrawable Members
        public virtual void Draw(SharpDX.Direct3D11.Device device, EngineTime time, ref Matrix world, ref Matrix view, ref Matrix projection) {
            foreach (var c in this.OfType<IDrawable>())
                c.Draw(device, time, ref world, ref view, ref projection);
        }
        #endregion

        #region IUpdateable Members
        public void Update(EngineTime time) {
            foreach (var c in this.OfType<IUpdateable>())
                c.Update(time);
        }
        #endregion

        #region IContentComponent Members
        public bool IsLoaded {
            get { return _IsLoaded; }
        }

        public virtual void Load(ViewerEngine engine) {
            _Engine = engine;
            foreach (var c in this.OfType<IContentComponent>().Where(_ => !_.IsLoaded))
                c.Load(engine);
            _IsLoaded = true;
        }

        public void Unload() {
            Unload(true);
        }
        public virtual void Unload(bool includeChildren) {
            _IsLoaded = false;
            if (includeChildren) {
                foreach (var c in this.OfType<IContentComponent>().Where(_ => _.IsLoaded))
                    c.Unload();
            }
            _Engine = null;
        }

        #endregion
    }
}
