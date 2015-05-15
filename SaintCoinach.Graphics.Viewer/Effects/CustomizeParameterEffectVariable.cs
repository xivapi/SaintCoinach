using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Effects {
    using SharpDX;
    using SharpDX.Direct3D11;

    public class CustomizeParameterEffectVariable {
        #region Fields
        private EffectVectorVariable _SkinColorVar;
        private EffectVectorVariable _LipColorVar;
        private EffectVectorVariable _HairColorVar;
        private EffectVectorVariable _MeshColorVar;
        private EffectVectorVariable _LeftColorVar;
        private EffectVectorVariable _RightColorVar;
        #endregion

        #region Properties
        public Vector4 SkinColor {
            set { _SkinColorVar.Set(value); }
        }
        public Vector4 LipColor {
            set { _LipColorVar.Set(value); }
        }
        public Vector3 HairColor {
            set { _HairColorVar.Set(value); }
        }
        public Vector3 MeshColor {
            set { _MeshColorVar.Set(value); }
        }
        public Vector3 LeftColor {
            set { _LeftColorVar.Set(value); }
        }
        public Vector3 RightColor {
            set { _RightColorVar.Set(value); }
        }
        #endregion

        #region Constructor
        public CustomizeParameterEffectVariable(EffectBase effect) {
            _HairColorVar = effect.GetVariableByName("m_HairColor").AsVector();
            _MeshColorVar = effect.GetVariableByName("m_MeshColor").AsVector();
            _LeftColorVar = effect.GetVariableByName("m_LeftColor").AsVector();
            _RightColorVar = effect.GetVariableByName("m_RightColor").AsVector();
            _SkinColorVar = effect.GetVariableByName("m_SkinColor").AsVector();
            _LipColorVar = effect.GetVariableByName("m_LipColor").AsVector();
        }
        #endregion

        public void Set(Data.CustomizeParameters parmeters) {
            this.SkinColor = parmeters.SkinColor;
            this.LipColor = parmeters.LipColor;
            this.HairColor = parmeters.HairColor;
            this.MeshColor = parmeters.MeshColor;
            this.LeftColor = parmeters.LeftEyeColor;
            this.RightColor = parmeters.RightEyeColor;
        }
    }
}
