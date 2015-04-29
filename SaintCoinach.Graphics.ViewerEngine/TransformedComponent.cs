using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace SaintCoinach.Graphics {
    public class TransformedComponent : IDrawable, IContentComponent {
        #region Fields
        private IContentComponent _InnerContent;
        private IDrawable _InnerDrawable;
        private Matrix _Transformation;
        #endregion

        public TransformedComponent(IDrawable drawable, Matrix transformation) {
            _InnerDrawable = drawable;
            _InnerContent = drawable as IContentComponent;
            _Transformation = transformation;
        }

        #region IDrawable Members

        public void Draw(SharpDX.Direct3D11.Device device, EngineTime time, ref SharpDX.Matrix world, ref SharpDX.Matrix view, ref SharpDX.Matrix projection) {
            var transformedWorld = _Transformation * world;

            _InnerDrawable.Draw(device, time, ref transformedWorld, ref view, ref projection);
        }

        #endregion


        #region IContentComponent Members

        public bool IsLoaded {
            get { return _InnerContent == null ? true : _InnerContent.IsLoaded; }
        }

        public void Load(ViewerEngine engine) {
            if (_InnerContent != null && !_InnerContent.IsLoaded)
                _InnerContent.Load(engine);
        }

        public void Unload() {
            if (_InnerContent != null && _InnerContent.IsLoaded)
                _InnerContent.Unload();
        }

        #endregion
    }
}
