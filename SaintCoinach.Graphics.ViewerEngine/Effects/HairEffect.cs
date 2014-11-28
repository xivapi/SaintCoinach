using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics.Effects {
    public class HairEffect : XivEffect {
        #region Static
        private static CompilationResult _EffectByteCode;

        private static CompilationResult EffectByteCode {
            get {
                if (_EffectByteCode == null)
                    _EffectByteCode = ShaderBytecode.CompileFromFile(
                        System.IO.Path.Combine("Effects", "HLSL", "Hair.fx"),
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

        private EffectVectorVariable _HairColorVar;
        private EffectVectorVariable _MeshColorVar;
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

        public Vector3 HairColor {
            get { return _HairColorVar.GetVector<Vector3>(); }
            set { _HairColorVar.Set(value); }
        }
        public Vector3 MeshColor {
            get { return _MeshColorVar.GetVector<Vector3>(); }
            set { _MeshColorVar.Set(value); }
        }

        public override Type RequiredVertexType {
            get { return typeof(Primitives.VertexCommon); }
        }
        #endregion

        #region Constructor
        public HairEffect(Device device) : this(device, EffectFlags.None) { }
        public HairEffect(Device device, EffectFlags flags)
            : base(device, EffectByteCode, flags) {
            Init();
        }
        #endregion

        #region Init
        private void Init() {
            _DiffuseVar = new EffectTextureVariable(this, "g_Diffuse");
            _MaskVar = new EffectTextureVariable(this, "g_Mask");
            _NormalVar = new EffectTextureVariable(this, "g_Normal");

            _HairColorVar = GetVariableByName("m_HairColor").AsVector();
            _MeshColorVar = GetVariableByName("m_MeshColor").AsVector();

            HairColor = new Vector3(1f, 0f, 0f);
            MeshColor = new Vector3(0f, 1f, 1f);
        }
        #endregion
    }
}
