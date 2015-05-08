using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    public abstract class Drawable3DComponent : Component, IDrawable3DComponent {
        #region Fields
        private Engine _Engine;
        #endregion

        #region Properties
        public Engine Engine { get { return _Engine; } }
        #endregion

        #region Constructor
        protected Drawable3DComponent(Engine engine) {
            _Engine = engine;
        }
        #endregion

        #region IDrawable3DComponent Members

        private bool _IsVisible = true;
        public bool IsVisible { get { return _IsVisible; } set { _IsVisible = value; } }

        public abstract void Draw(EngineTime time, ref SharpDX.Matrix world, ref SharpDX.Matrix view, ref SharpDX.Matrix projection);

        #endregion
    }
}
