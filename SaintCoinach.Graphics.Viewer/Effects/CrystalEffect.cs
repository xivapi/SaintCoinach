using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Effects {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;

    public class CrystalEffect : EffectBase {
        #region Fields
        private EffectShaderResourceVariable _ColorMap0Var;
        private EffectShaderResourceVariable _SpecularMap0Var;
        private EffectShaderResourceVariable _NormalMap0Var;
        private EffectShaderResourceVariable _EnvironmentMap;
        #endregion

        #region Properties
        public ShaderResourceView ColorMap0 {
            set { _ColorMap0Var.SetResource(value); }
        }
        public ShaderResourceView SpecularMap0 {
            set { _SpecularMap0Var.SetResource(value); }
        }
        public ShaderResourceView NormalMap0 {
            set { _NormalMap0Var.SetResource(value); }
        }
        public ShaderResourceView EnvironmentMap {
            set { _EnvironmentMap.SetResource(value); }
        }
        #endregion

        #region Constructor
        public CrystalEffect(Device device, byte[] byteCode) : this(device, byteCode, EffectFlags.None) { }
        public CrystalEffect(Device device, byte[] byteCode, EffectFlags flags)
            : base(device, byteCode, flags) {
            Init();
        }
        #endregion

        #region Init
        private void Init() {
            _ColorMap0Var = GetVariableByName("g_ColorMap0").AsShaderResource();
            _SpecularMap0Var = GetVariableByName("g_SpecularMap0").AsShaderResource();
            _NormalMap0Var = GetVariableByName("g_NormalMap0").AsShaderResource();
            _EnvironmentMap = GetVariableByName("g_EnvMap").AsShaderResource();
        }
        #endregion
    }
}
