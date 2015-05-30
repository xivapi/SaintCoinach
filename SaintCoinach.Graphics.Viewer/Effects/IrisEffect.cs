using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Effects {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;

    public class IrisEffect : SkinnedEffect {
        #region Fields
        private EffectShaderResourceVariable _NormalVar;
        private EffectShaderResourceVariable _MaskVar;
        private CustomizeParameterEffectVariable _CustomizeParameterVar;
        #endregion

        #region Properties
        public ShaderResourceView Normal {
            set { _NormalVar.SetResource(value); }
        }
        public ShaderResourceView Mask {
            set { _MaskVar.SetResource(value); }
        }
        public Data.CustomizeParameters CustomizeParameters {
            set { _CustomizeParameterVar.Set(value); }
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

            _CustomizeParameterVar = new CustomizeParameterEffectVariable(this);
        }
        #endregion
    }
}
