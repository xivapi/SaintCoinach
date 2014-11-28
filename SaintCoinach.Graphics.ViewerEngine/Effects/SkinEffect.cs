using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics.Effects {
    public class SkinEffect : XivEffect {
        #region Static
        private static CompilationResult _EffectByteCode;

        private static CompilationResult EffectByteCode {
            get {
                if (_EffectByteCode == null)
                    _EffectByteCode = ShaderBytecode.CompileFromFile(
                        System.IO.Path.Combine("Effects", "HLSL", "Skin.fx"),
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
        private EffectTextureVariable _DiffuseVar;
        private EffectTextureVariable _MaskVar;
        private EffectTextureVariable _NormalVar;

        private EffectVectorVariable _SkinColorVar;
        private EffectVectorVariable _LipColorVar;
        #endregion

        #region Properties
        public ShaderResourceView Diffuse {
            set { _DiffuseVar.Texture = value; }
        }
        public ShaderResourceView Mask {
            set { _MaskVar.Texture = value; }
        }
        public ShaderResourceView Normal {
            set { _NormalVar.Texture = value; }
        }

        public Vector4 SkinColor {
            get { return _SkinColorVar.GetVector<Vector4>(); }
            set { _SkinColorVar.Set(value); }
        }
        public Vector4 LipColor {
            get { return _LipColorVar.GetVector<Vector4>(); }
            set { _LipColorVar.Set(value); }
        }

        public override Type RequiredVertexType {
            get { return typeof(Primitives.VertexCommon); }
        }
        #endregion

        #region Constructor
        public SkinEffect(Device device) : this(device, EffectFlags.None) { }
        public SkinEffect(Device device, EffectFlags flags)
            : base(device, EffectByteCode, flags) {
            Init();
        }
        #endregion

        #region Init
        private void Init() {
            _DiffuseVar = new EffectTextureVariable(this, "g_Diffuse");
            _MaskVar = new EffectTextureVariable(this, "g_Mask");
            _NormalVar = new EffectTextureVariable(this, "g_Normal");

            _SkinColorVar = GetVariableByName("m_SkinColor").AsVector();
            _LipColorVar = GetVariableByName("m_LipColor").AsVector();

            SkinColor = new Vector4(1f, 0f, 0f, 1f);
            LipColor = new Vector4(0f, 0f, 1f, 0.5f);
        }
        #endregion
    }
}
