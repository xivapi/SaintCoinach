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
        public Texture2D Diffuse0 {
            set { _Diffuse0Var.Texture = value; }
        }
        public Texture2D Specular0 {
            set { _Specular0Var.Texture = value; }
        }
        public Texture2D Normal0 {
            set { _Normal0Var.Texture = value; }
        }
        public Texture2D Diffuse1 {
            set { _Diffuse1Var.Texture = value; }
        }
        public Texture2D Specular1 {
            set { _Specular1Var.Texture = value; }
        }
        public Texture2D Normal1 {
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
            _Diffuse0Var = new EffectTextureVariable(this, "Diffuse0");
            _Specular0Var = new EffectTextureVariable(this, "Specular0");
            _Normal0Var = new EffectTextureVariable(this, "Normal0");
            _Diffuse1Var = new EffectTextureVariable(this, "Diffuse1");
            _Specular1Var = new EffectTextureVariable(this, "Specular1");
            _Normal1Var = new EffectTextureVariable(this, "Normal1");

            var texSampleState = new SamplerState(Device, new SamplerStateDescription {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                Filter = Filter.Anisotropic,
                BorderColor = Color.Purple,
                ComparisonFunction = Comparison.Never,
                MaximumAnisotropy = 16,
                MinimumLod = 0,
                MaximumLod = 16,
                MipLodBias = 0
            });
            _Diffuse0Var.SamplerState = texSampleState;
            _Specular0Var.SamplerState = texSampleState;
            _Normal0Var.SamplerState = texSampleState;

            _Diffuse1Var.SamplerState = texSampleState;
            _Specular1Var.SamplerState = texSampleState;
            _Normal1Var.SamplerState = texSampleState;
        }
        #endregion
    }
}
