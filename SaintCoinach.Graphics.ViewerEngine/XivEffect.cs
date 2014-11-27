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
        private EffectVectorVariable _EmissiveColorVar;
        private EffectVectorVariable _AmbientColorVar;
        private EffectMatrixVariable _WorldVar;
        private EffectMatrixVariable _WorldInverseTransposeVar;
        private EffectMatrixVariable _WorldViewProjectionVar;
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
        public Vector3 EmissiveColor {
            get { return _EmissiveColorVar.GetVector<Vector3>(); }
            set { _EmissiveColorVar.Set(value); }
        }
        public Vector3 AmbientColor {
            get { return _AmbientColorVar.GetVector<Vector3>(); }
            set { _AmbientColorVar.Set(value); }
        }
        public Matrix World {
            get { return _WorldVar.GetMatrix(); }
            set { _WorldVar.SetMatrix(value); }
        }
        public Matrix WorldInverseTranspose {
            get { return _WorldInverseTransposeVar.GetMatrix(); }
            set { _WorldInverseTransposeVar.SetMatrix(value); }
        }
        public Matrix WorldViewProjection {
            get { return _WorldViewProjectionVar.GetMatrix(); }
            set { _WorldViewProjectionVar.SetMatrix(value); }
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
            _EmissiveColorVar = GetVariableByName("EmissiveColor").AsVector();
            _AmbientColorVar = GetVariableByName("AmbientColor").AsVector();
            _WorldVar = GetVariableByName("World").AsMatrix();
            _WorldInverseTransposeVar = GetVariableByName("WorldInverseTranspose").AsMatrix();
            _WorldViewProjectionVar = GetVariableByName("WorldViewProj").AsMatrix();

            Light = new PointLight {
                Direction = new Vector3(-0.5265408f, -0.5735765f, -0.6275069f),
                Diffuse = new Vector3(1, 0.9607844f, 0.8078432f),
                Specular = new Vector3(1, 0.9607844f, 0.8078432f),
            };
            AmbientColor = new Vector3(0.05333332f, 0.09882354f, 0.1819608f);
        }
        #endregion

        #region Apply
        public void Apply(ref Matrix world, ref Matrix view, ref Matrix projection) {
            this.World = world;

            Matrix worldTranspose;
            Matrix worldInverseTranspose;
            Matrix.Invert(ref world, out worldTranspose);
            Matrix.Transpose(ref worldTranspose, out worldInverseTranspose);
            this.WorldInverseTranspose = worldInverseTranspose;

            Matrix viewProjection;
            Matrix worldViewProjection;
            Matrix.Multiply(ref view, ref projection, out viewProjection);
            Matrix.Multiply(ref world, ref viewProjection, out worldViewProjection);
            this.WorldViewProjection = worldViewProjection;

            Matrix viewInverse;
            Matrix.Invert(ref view, out viewInverse);

            EyePosition = viewInverse.TranslationVector;
        }
        #endregion
    }
}
