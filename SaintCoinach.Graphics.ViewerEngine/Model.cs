using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace SaintCoinach.Graphics {
    public class Model : IDrawable, IContentComponent {
        #region Fields
        private Assets.SubModel _SourceModel;
        private Parts.Mesh[] _Meshes;
        private bool _IsLoaded = false;
        #endregion

        #region Properties
        public IEnumerable<Parts.Mesh> Meshes { get { return _Meshes; } }
        public Matrix Transform { get; set; }
        #endregion

        #region Constructor
        public Model(Assets.SubModel sourceModel) : this(sourceModel, Matrix.Identity) { }
        public Model(Assets.SubModel sourceModel, Matrix transform) {
            _SourceModel = sourceModel;

            _Meshes = sourceModel.Meshes.Select(_ => new Parts.Mesh(_)).ToArray();
        }
        #endregion

        #region IDrawable Members
        public void Draw(SharpDX.Direct3D11.Device device, EngineTime time, ref Matrix world, ref Matrix view, ref Matrix projection) {
            Matrix adjustedWorld = Transform * world;

            foreach (var mesh in _Meshes)
                mesh.Draw(device, time, ref adjustedWorld, ref view, ref projection);
        }
        #endregion

        public bool SetMaterialVersion(int v) {
            var result = true;
            foreach (var mesh in Meshes) {
                if (mesh.AvailableMaterialVersions.Contains(v))
                    mesh.MaterialVersion = v;
                else
                    result = false;
            }
            return result;
        }
        public bool SetMaterialStain(int stain) {
            var result = true;
            foreach (var mesh in Meshes) {
                if (mesh.CanStain && mesh.AvailableStains.Contains(stain))
                    mesh.MaterialStain = stain;
                else
                    result = false;
            }
            return result;
        }

        #region IContentComponent Members
        public bool IsLoaded {
            get { return _IsLoaded; }
        }

        public void Load(SharpDX.Direct3D11.Device device) {
            foreach (var mesh in _Meshes)
                mesh.Load(device);
            _IsLoaded = true;
        }

        public void Unload() {
            foreach (var mesh in _Meshes)
                mesh.Unload();
            _IsLoaded = false;
        }
        #endregion
    }
}
