using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Effects {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;

    public class SkinEffect : EffectBase {
        #region Fields
        private EffectShaderResourceVariable _DiffuseVar;
        private EffectShaderResourceVariable _NormalVar;
        private EffectShaderResourceVariable _MaskVar;
        private EffectVectorVariable _SkinColorVar;
        private EffectVectorVariable _LipColorVar;
        #endregion

        #region Properties
        public ShaderResourceView Diffuse {
            set { _DiffuseVar.SetResource(value); }
        }
        public ShaderResourceView Normal {
            set { _NormalVar.SetResource(value); }
        }
        public ShaderResourceView Mask {
            set { _MaskVar.SetResource(value); }
        }
        public Vector4 SkinColor {
            set { _SkinColorVar.Set(value); }
        }
        public Vector4 LipColor {
            set { _LipColorVar.Set(value); }
        }
        #endregion

        #region Constructor
        public SkinEffect(Device device, byte[] byteCode) : this(device, byteCode, EffectFlags.None) { }
        public SkinEffect(Device device, byte[] byteCode, EffectFlags flags)
            : base(device, byteCode, flags) {
            Init();
        }
        #endregion

        #region Init
        private void Init() {
            _DiffuseVar = GetVariableByName("g_Diffuse").AsShaderResource();
            _NormalVar = GetVariableByName("g_Normal").AsShaderResource();
            _MaskVar = GetVariableByName("g_Mask").AsShaderResource();

            _SkinColorVar = GetVariableByName("m_SkinColor").AsVector();
            _LipColorVar = GetVariableByName("m_LipColor").AsVector();
        }
        #endregion
    }
}
