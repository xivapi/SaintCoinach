using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Effects {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;

    public class EffectTextureVariable {
        #region Fields
        private Effect _Effect;
        private EffectShaderResourceVariable _TextureVar;
        private EffectSamplerVariable _SamplerVar;
        #endregion

        #region Properties
        public SamplerState SamplerState {
            get { return _SamplerVar.GetSampler(); }
            set { _SamplerVar.SetSampler(0, value); }
        }
        public ShaderResourceView Texture {
            set { _TextureVar.SetResource(value); }
        }
        #endregion

        #region Constructor
        public EffectTextureVariable(Effect effect, string name) {
            _Effect = effect;
            _TextureVar = effect.GetVariableByName(name).AsShaderResource();
            _SamplerVar = effect.GetVariableByName(name + "Sampler").AsSampler();
        }
        #endregion
    }
}
