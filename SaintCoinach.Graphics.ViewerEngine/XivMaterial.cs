using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics {
    public abstract class XivMaterial : IDisposable {
        #region Fields
        private XivEffect _Effect;
        private Assets.MaterialVersion _Material;
        private EffectTechnique _CurrentTechnique;
        private string _CurrentTechniqueName;
        #endregion

        #region Properties
        public XivEffect Effect { get { return _Effect; } }
        public EffectTechnique CurrentTechnique {
            get { return _CurrentTechnique; }
            set {
                _CurrentTechnique = value;
                _CurrentTechniqueName = value.Description.Name;
            }
        }
        public string CurrentTechniqueName {
            get { return _CurrentTechniqueName; }
            set {
                _CurrentTechniqueName = value;
                _CurrentTechnique = Effect.GetTechniqueByName(value);
            }
        }
        #endregion

        #region Constructor
        protected XivMaterial(Device device, Assets.MaterialVersion material, XivEffect effect) {
            _Effect = effect;
            _Material = material;
        }
        #endregion

        #region Apply
        public abstract void Apply();
        #endregion

        public static XivMaterial Create(Device device, Assets.MaterialVersion material) {
            switch (material.Shader) {
                case "character.shpk":
                    return new Materials.CharacterMaterial(device, material);
            }
            throw new NotSupportedException();
        }

        #region IDisposable Members

        public void Dispose() {
            Dispose(true);
        }
        protected virtual void Dispose(bool managed) {
            if (_Effect != null)
                _Effect.Dispose();
            _Effect = null;
        }

        #endregion
    }
}
