using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics.Viewer {
    public abstract class XivEffect : Effect {
        #region Effect variables
        private EffectVectorVariable _EyePositionVar;
        private EffectPointLightVariable _LightVar;
        private EffectVariable _EmissiveColorVar;
        private EffectVariable _AmbientColorVar;
        private EffectVariable _WorldVar;
        private EffectVariable _WorldInverseTransposeVar;
        private EffectVariable _WorldViewProjectionVar;
        #endregion

        #region Variable values
        public Vector3 EyePosition {
            get { return _EyePositionVar.GetVector<Vector3>(); }
            set { _EyePositionVar.Set(value); }
        }
        public PointLight Light {
            get { return _LightVar.Get(); }
            set { _LightVar.Set(value); }
        }
        #endregion

        #region Constructor
        protected XivEffect(Device device, byte[] byteCode) : this(device, byteCode, EffectFlags.None) { }
        protected XivEffect(Device device, byte[] byteCode, EffectFlags flags)
            : base(device, byteCode, flags) {

            Init();
        }
        #endregion

        #region Init
        private void Init() {
            _EyePositionVar = GetVariableByName("EyePosition").AsVector();
            _LightVar = new EffectPointLightVariable(GetVariableByName("Light"));
            _EmissiveColorVar = GetVariableByName("EmissiveColor");
            _AmbientColorVar = GetVariableByName("AmbientColor");
            _WorldVar = GetVariableByName("World");
            _WorldInverseTransposeVar = GetVariableByName("WorldInverseTranspose");
            _WorldViewProjectionVar = GetVariableByName("WorldViewProj");

            
        }
        #endregion
    }
}
