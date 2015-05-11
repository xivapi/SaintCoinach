using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Effects {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;

    public class IrisEffect : EffectBase {
        #region Fields
        private EffectShaderResourceVariable _NormalVar;
        private EffectShaderResourceVariable _MaskVar;
        private EffectVectorVariable _LeftColorVar;
        private EffectVectorVariable _RightColorVar;
        #endregion

        #region Properties
        public ShaderResourceView Normal {
            set { _NormalVar.SetResource(value); }
        }
        public ShaderResourceView Mask {
            set { _MaskVar.SetResource(value); }
        }
        public Vector3 LeftColor {
            set { _LeftColorVar.Set(value); }
        }
        public Vector3 RightColor {
            set { _RightColorVar.Set(value); }
        }
        #endregion

        #region Constructor
        public IrisEffect(Device device, byte[] byteCode) : this(device, byteCode, EffectFlags.None) { }
        public IrisEffect(Device device, byte[] byteCode, EffectFlags flags)
            : base(device, byteCode, flags) {
            Init();
        }
        #endregion

        #region Init
        private void Init() {
            _NormalVar = GetVariableByName("g_Normal").AsShaderResource();
            _MaskVar = GetVariableByName("g_Mask").AsShaderResource();

            _LeftColorVar = GetVariableByName("m_LeftColor").AsVector();
            _RightColorVar = GetVariableByName("m_RightColor").AsVector();
        }
        #endregion
    }
}
