using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Parts {
    public class Mesh : IDrawable, IContentComponent {
        #region Fields
        private Assets.Mesh _SourceMesh;
        private bool _IsLoaded = false;
        #endregion

        #region Constructor
        public Mesh(Assets.Mesh sourceMesh) {
            _SourceMesh = sourceMesh;
        }
        #endregion

        #region IDrawable Members

        public void Draw(SharpDX.Direct3D11.Device device, EngineTime time) {
            throw new NotImplementedException();
        }

        #endregion

        #region IContentComponent Members

        public bool IsLoaded {
            get { return _IsLoaded; }
        }

        public void Load(SharpDX.Direct3D11.Device device) {
            throw new NotImplementedException();
            _IsLoaded = true;
        }

        public void Unload() {
            throw new NotImplementedException();
            _IsLoaded = false;
        }

        #endregion
    }
}
