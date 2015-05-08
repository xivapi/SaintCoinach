using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    public class ComponentContainer : System.Collections.ObjectModel.Collection<IComponent>, IDrawable3DComponent, IUpdateableComponent, IContentComponent {
        #region IDrawable3DComponent Members

        private bool _IsVisible = true;
        public bool IsVisible { get { return _IsVisible; } set { _IsVisible = value; } }

        public void Draw(EngineTime time, ref SharpDX.Matrix world, ref SharpDX.Matrix view, ref SharpDX.Matrix projection) {
            foreach (var component in this.OfType<IDrawable3DComponent>().Where(c => c.IsVisible).ToArray())
                component.Draw(time, ref world, ref view, ref projection);
        }

        #endregion

        #region IUpdateableComponent Members

        private bool _IsEnabled = true;
        public bool IsEnabled { get { return _IsEnabled; } set { _IsEnabled = value; } }

        public void Update(EngineTime engineTime) {
            foreach (var component in this.OfType<IUpdateableComponent>().Where(c => c.IsEnabled).ToArray())
                component.Update(engineTime);
        }

        #endregion

        #region IContentComponent Members

        public bool IsLoaded {
            get;
            private set;
        }

        public void LoadContent() {
            IsLoaded = true;
            foreach (var component in this.OfType<IContentComponent>().Where(c => !c.IsLoaded).ToArray())
                component.LoadContent();
        }

        public void UnloadContent() {
            IsLoaded = false;
            foreach (var component in this.OfType<IContentComponent>().Where(c => c.IsLoaded).ToArray())
                component.LoadContent();
        }

        #endregion

        #region Overrides
        protected override void InsertItem(int index, IComponent item) {
            base.InsertItem(index, item);

            var asContent = item as IContentComponent;
            if (asContent != null && this.IsLoaded && !asContent.IsLoaded)
                asContent.LoadContent();
        }
        protected override void SetItem(int index, IComponent item) {
            base.SetItem(index, item);

            var asContent = item as IContentComponent;
            if (asContent != null && this.IsLoaded && !asContent.IsLoaded)
                asContent.LoadContent();
        }
        #endregion
    }
}
