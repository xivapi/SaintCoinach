using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Effects {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;

    public class BgEffect : EffectBase {
        #region Fields
        private EffectShaderResourceVariable _Diffuse0Var;
        private EffectShaderResourceVariable _Specular0Var;
        private EffectShaderResourceVariable _Normal0Var;
        private EffectShaderResourceVariable _Diffuse1Var;
        private EffectShaderResourceVariable _Specular1Var;
        private EffectShaderResourceVariable _Normal1Var;
        #endregion

        #region Properties
        public ShaderResourceView Diffuse0 {
            set { _Diffuse0Var.SetResource(value); }
        }
        public ShaderResourceView Specular0 {
            set { _Specular0Var.SetResource(value); }
        }
        public ShaderResourceView Normal0 {
            set { _Normal0Var.SetResource(value); }
        }
        public ShaderResourceView Diffuse1 {
            set { _Diffuse1Var.SetResource(value); }
        }
        public ShaderResourceView Specular1 {
            set { _Specular1Var.SetResource(value); }
        }
        public ShaderResourceView Normal1 {
            set { _Normal1Var.SetResource(value); }
        }
        #endregion

        #region Constructor
        public BgEffect(Device device, byte[] byteCode) : this(device, byteCode, EffectFlags.None) { }
        public BgEffect(Device device, byte[] byteCode, EffectFlags flags)
            : base(device, byteCode, flags) {
            Init();
        }
        #endregion

        #region Init
        private void Init() {
            _Diffuse0Var = GetVariableByName("g_Diffuse0").AsShaderResource();
            _Specular0Var = GetVariableByName("g_Specular0").AsShaderResource();
            _Normal0Var = GetVariableByName("g_Normal0").AsShaderResource();
            _Diffuse1Var = GetVariableByName("g_Diffuse1").AsShaderResource();
            _Specular1Var = GetVariableByName("g_Specular1").AsShaderResource();
            _Normal1Var = GetVariableByName("g_Normal1").AsShaderResource();
        }
        #endregion
    }
}
