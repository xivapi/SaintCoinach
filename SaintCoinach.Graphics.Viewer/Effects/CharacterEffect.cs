using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Effects {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;

    public class CharacterEffect : SkinnedEffect {
        #region Fields
        private EffectShaderResourceVariable _DiffuseVar;
        private EffectShaderResourceVariable _SpecularVar;
        private EffectShaderResourceVariable _NormalVar;
        private EffectShaderResourceVariable _MaskVar;
        private EffectShaderResourceVariable _TableVar;
        #endregion

        #region Properties
        public ShaderResourceView Diffuse {
            set { _DiffuseVar.SetResource(value); }
        }
        public ShaderResourceView Specular {
            set { _SpecularVar.SetResource(value); }
        }
        public ShaderResourceView Normal {
            set { _NormalVar.SetResource(value); }
        }
        public ShaderResourceView Mask {
            set { _MaskVar.SetResource(value); }
        }
        public ShaderResourceView Table {
            set { _TableVar.SetResource(value); }
        }
        #endregion

        #region Constructor
        public CharacterEffect(Device device, byte[] byteCode) : this(device, byteCode, EffectFlags.None) { }
        public CharacterEffect(Device device, byte[] byteCode, EffectFlags flags)
            : base(device, byteCode, flags) {
            Init();
        }
        #endregion

        #region Init
        private void Init() {
            _DiffuseVar = GetVariableByName("g_Diffuse").AsShaderResource();
            _SpecularVar = GetVariableByName("g_Specular").AsShaderResource();
            _NormalVar = GetVariableByName("g_Normal").AsShaderResource();
            _MaskVar = GetVariableByName("g_Mask").AsShaderResource();
            _TableVar = GetVariableByName("g_Table").AsShaderResource();
        }
        #endregion
    }
}
