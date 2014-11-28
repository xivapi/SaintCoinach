using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics.Effects {
    public class BgEffect : XivEffect {
        #region Static
        private static CompilationResult _EffectByteCode;

        private static CompilationResult EffectByteCode {
            get {
                if (_EffectByteCode == null)
                    _EffectByteCode = ShaderBytecode.CompileFromFile(
                        System.IO.Path.Combine("Effects", "HLSL", "Bg.fx"),
                        "fx_5_0",
                        ShaderFlags.None,
                        EffectFlags.None,
                        new SharpDX.Direct3D.ShaderMacro[] { new SharpDX.Direct3D.ShaderMacro("SM4", "SM4") },
                        new ShaderCompilerInclude(System.IO.Path.Combine("Effects", "HLSL")));
                return _EffectByteCode;
            }
        }
        #endregion

        #region Fields
        private EffectTextureVariable _Diffuse0Var;
        private EffectTextureVariable _Specular0Var;
        private EffectTextureVariable _Normal0Var;

        private EffectTextureVariable _Diffuse1Var;
        private EffectTextureVariable _Specular1Var;
        private EffectTextureVariable _Normal1Var;
        #endregion

        #region Properties
        public ShaderResourceView Diffuse0 {
            set { _Diffuse0Var.Texture = value; }
        }
        public ShaderResourceView Specular0 {
            set { _Specular0Var.Texture = value; }
        }
        public ShaderResourceView Normal0 {
            set { _Normal0Var.Texture = value; }
        }
        public ShaderResourceView Diffuse1 {
            set { _Diffuse1Var.Texture = value; }
        }
        public ShaderResourceView Specular1 {
            set { _Specular1Var.Texture = value; }
        }
        public ShaderResourceView Normal1 {
            set { _Normal1Var.Texture = value; }
        }
        public override Type RequiredVertexType {
            get { return typeof(Primitives.VertexDualTexture); }
        }
        #endregion

        #region Constructor
        public BgEffect(Device device) : this(device, EffectFlags.None) { }
        public BgEffect(Device device, EffectFlags flags)
            : base(device, EffectByteCode, flags) {
            Init();
        }
        #endregion

        #region Init
        private void Init() {
            _Diffuse0Var = new EffectTextureVariable(this, "g_Diffuse0");
            _Diffuse1Var = new EffectTextureVariable(this, "g_Diffuse1");

            _Normal0Var = new EffectTextureVariable(this, "g_Normal0");
            _Normal1Var = new EffectTextureVariable(this, "g_Normal1");

            _Specular0Var = new EffectTextureVariable(this, "g_Specular0");
            _Specular1Var = new EffectTextureVariable(this, "g_Specular1");
        }
        #endregion
    }
}
