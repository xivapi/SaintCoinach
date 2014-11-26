using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    public class Model : IDrawable, IContentComponent {
        #region Fields
        private Assets.SubModel _SourceModel;
        private Parts.Mesh[] _Meshes;
        private bool _IsLoaded = false;
        #endregion

        #region Constructor
        public Model(Assets.SubModel sourceModel) {
            _SourceModel = sourceModel;

            _Meshes = sourceModel.Meshes.Select(_ => new Parts.Mesh(_)).ToArray();
        }
        #endregion

        #region IDrawable Members
        public void Draw(SharpDX.Direct3D11.Device device, EngineTime time) {
            throw new NotImplementedException();

            foreach (var mesh in _Meshes)
                mesh.Draw(device, time);
        }
        #endregion

        #region IContentComponent Members
        public bool IsLoaded {
            get { return _IsLoaded; }
        }

        public void Load(SharpDX.Direct3D11.Device device) {
            throw new NotImplementedException();
            foreach (var mesh in _Meshes)
                mesh.Load(device);
            _IsLoaded = true;
        }

        public void Unload() {
            throw new NotImplementedException();
            foreach (var mesh in _Meshes)
                mesh.Unload();
            _IsLoaded = false;
        }
        #endregion
    }
}
