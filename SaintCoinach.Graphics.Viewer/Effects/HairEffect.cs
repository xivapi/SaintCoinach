using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Effects {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;

    public class HairEffect : EffectBase {
        #region Fields
        private EffectShaderResourceVariable _NormalVar;
        private EffectShaderResourceVariable _MaskVar;
        private EffectVectorVariable _HairColorVar;
        private EffectVectorVariable _MeshColorVar;
        #endregion

        #region Properties
        public ShaderResourceView Normal {
            set { _NormalVar.SetResource(value); }
        }
        public ShaderResourceView Mask {
            set { _MaskVar.SetResource(value); }
        }
        public Vector3 HairColor {
            set { _HairColorVar.Set(value); }
        }
        public Vector3 MeshColor {
            set { _MeshColorVar.Set(value); }
        }
        #endregion

        #region Constructor
        public HairEffect(Device device, byte[] byteCode) : this(device, byteCode, EffectFlags.None) { }
        public HairEffect(Device device, byte[] byteCode, EffectFlags flags)
            : base(device, byteCode, flags) {
            Init();
        }
        #endregion

        #region Init
        private void Init() {
            _NormalVar = GetVariableByName("g_Normal").AsShaderResource();
            _MaskVar = GetVariableByName("g_Mask").AsShaderResource();

            _HairColorVar = GetVariableByName("m_HairColor").AsVector();
            _MeshColorVar = GetVariableByName("m_MeshColor").AsVector();
        }
        #endregion
    }
}
