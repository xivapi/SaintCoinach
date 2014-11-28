using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics.Effects {
    public class CharacterEffect : XivEffect {
        #region Static
        private static CompilationResult _EffectByteCode;

        private static CompilationResult EffectByteCode {
            get {
                if (_EffectByteCode == null)
                    _EffectByteCode = ShaderBytecode.CompileFromFile(
                        System.IO.Path.Combine("Effects", "HLSL", "Character.fx"),
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
        private EffectTextureVariable _SpecularVar;
        private EffectTextureVariable _NormalVar;
        private EffectTextureVariable _MaskVar;
        private EffectTextureVariable _TableVar;
        #endregion

        #region Properties
        public Texture2D Diffuse {
            set { _DiffuseVar.Texture = value; }
        }
        public Texture2D Specular {
            set { _SpecularVar.Texture = value; }
        }
        public Texture2D Normal {
            set { _NormalVar.Texture = value; }
        }
        public Texture2D Mask {
            set { _MaskVar.Texture = value; }
        }
        public Texture2D Table {
            set { _TableVar.Texture = value; }
        }
        public override Type RequiredVertexType {
            get { return typeof(Primitives.VertexCommon); }
        }
        #endregion

        #region Constructor
        public CharacterEffect(Device device) : this(device, EffectFlags.None) { }
        public CharacterEffect(Device device, EffectFlags flags) : base(device, EffectByteCode, flags) {
            Init();
        }
        #endregion

        #region Init
        private void Init() {
            _DiffuseVar = new EffectTextureVariable(this, "g_Diffuse");
            _SpecularVar = new EffectTextureVariable(this, "g_Specular");
            _NormalVar = new EffectTextureVariable(this, "g_Normal");
            _MaskVar = new EffectTextureVariable(this, "g_Mask");
            _TableVar = new EffectTextureVariable(this, "g_Table");
        }
        #endregion
    }
}
