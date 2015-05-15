using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Content {
    using SharpDX;
    using SharpDX.Direct3D11;

    public abstract class MaterialBase {
        #region Properties
        private EffectTechnique _CurrentTechnique;
        private string _CurrentTechniqueName;

        public Engine Engine { get; private set; }
        public Material BaseMaterial { get; private set; }
        public Effects.EffectBase Effect { get; private set; }
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
        protected MaterialBase(Engine engine, Material baseMaterial) {
            this.Engine = engine;
            this.BaseMaterial = baseMaterial;

            this.Effect = engine.EffectFactory.GetEffect(BaseMaterial.Shader);
        }
        #endregion

        #region Apply
        public abstract void Apply(Data.ParametersBase parameters);
        #endregion
    }
}
