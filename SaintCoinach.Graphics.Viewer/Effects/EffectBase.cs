using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Effects {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;

    public abstract class EffectBase : Effect {
        public static readonly ReadOnlyCollection<string> SupportedShaders = new ReadOnlyCollection<string>(new List<string> {
            "character.shpk",
            "bg.shpk",
            "skin.shpk",
            "hair.shpk"
        });

        #region Effect variables
        private EffectVectorVariable _EyePositionVar;

        private EffectDirectionalLightVariable _DirectionalLight0Var;
        private EffectDirectionalLightVariable _DirectionalLight1Var;
        private EffectDirectionalLightVariable _DirectionalLight2Var;

        private EffectVectorVariable _DiffuseColorVar;
        private EffectVectorVariable _EmissiveColorVar;
        private EffectVectorVariable _AmbientColorVar;
        private EffectVectorVariable _SpecularColorVar;
        private EffectScalarVariable _SpecularPowerVar;

        private EffectMatrixVariable _WorldVar;
        private EffectMatrixVariable _WorldInverseTransposeVar;
        private EffectMatrixVariable _WorldViewProjectionVar;
        private EffectMatrixVariable _ViewInverseVar;
        #endregion

        #region Variable values
        public Vector3 EyePosition {
            get { return _EyePositionVar.GetVector<Vector3>(); }
            set { _EyePositionVar.Set(value); }
        }
        public DirectionalLight Light0 {
            get { return _DirectionalLight0Var.Get(); }
            set { _DirectionalLight0Var.Set(value); }
        }
        public DirectionalLight Light1 {
            get { return _DirectionalLight1Var.Get(); }
            set { _DirectionalLight1Var.Set(value); }
        }
        public DirectionalLight Light2 {
            get { return _DirectionalLight2Var.Get(); }
            set { _DirectionalLight2Var.Set(value); }
        }

        public Vector4 DiffuseColor {
            get { return _DiffuseColorVar.GetVector<Vector4>(); }
            set { _DiffuseColorVar.Set(value); }
        }
        public Vector3 EmissiveColor {
            get { return _EmissiveColorVar.GetVector<Vector3>(); }
            set { _EmissiveColorVar.Set(value); }
        }
        public Vector3 AmbientColor {
            get { return _AmbientColorVar.GetVector<Vector3>(); }
            set { _AmbientColorVar.Set(value); }
        }
        public Vector3 SpecularColor {
            get { return _SpecularColorVar.GetVector<Vector3>(); }
            set { _SpecularColorVar.Set(value); }
        }
        public float SpecularPower {
            get { return _SpecularPowerVar.GetFloat(); }
            set { _SpecularPowerVar.Set(value); }
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
        public Matrix ViewInverse {
            get { return _ViewInverseVar.GetMatrix(); }
            set { _ViewInverseVar.SetMatrix(value); }
        }
        #endregion

        #region Constructor
        protected EffectBase(Device device, byte[] byteCode) : this(device, byteCode, EffectFlags.None) { }
        protected EffectBase(Device device, byte[] byteCode, EffectFlags flags)
            : base(device, byteCode, flags) {

            Init();
        }
        #endregion

        #region Init
        private void Init() {
            _WorldVar = GetVariableByName("g_World").AsMatrix();
            _WorldInverseTransposeVar = GetVariableByName("g_WorldInverseTranspose").AsMatrix();
            _WorldViewProjectionVar = GetVariableByName("g_WorldViewProjection").AsMatrix();

            _ViewInverseVar = GetVariableByName("m_ViewInverse").AsMatrix();

            _EyePositionVar = GetVariableByName("m_EyePosition").AsVector();
            _DirectionalLight0Var = new EffectDirectionalLightVariable(GetVariableByName("m_Light0"));
            _DirectionalLight1Var = new EffectDirectionalLightVariable(GetVariableByName("m_Light1"));
            _DirectionalLight2Var = new EffectDirectionalLightVariable(GetVariableByName("m_Light2"));

            _DiffuseColorVar = GetVariableByName("m_DiffuseColor").AsVector();
            _EmissiveColorVar = GetVariableByName("m_EmissiveColor").AsVector();
            _AmbientColorVar = GetVariableByName("m_AmbientColor").AsVector();
            _SpecularColorVar = GetVariableByName("m_SpecularColor").AsVector();
            _SpecularPowerVar = GetVariableByName("m_SpecularPower").AsScalar();

            SetDefaults();
        }

        public virtual void SetDefaults() {
            SpecularColor = Vector3.One;
            SpecularPower = 64;

            DiffuseColor = Vector4.One;
            Light0 = new DirectionalLight {
                Direction = new Vector3(0.5f, 0.25f, 1),
                Diffuse = Vector4.One,
                Specular = Vector4.One * 0.75f,
            };
            Light1 = new DirectionalLight {
                Direction = new Vector3(0, -1, 0),
                Diffuse = Vector4.One,
                Specular = Vector4.One * 0.75f,
            };
            Light2 = new DirectionalLight {
                Direction = new Vector3(-0.5f, 0.25f, -1),
                Diffuse = Vector4.One,
                Specular = Vector4.One * 0.75f,
            };
            /*
            Light0 = new DirectionalLight {
                Direction = new Vector3(-0.5265408f, -0.5735765f, -0.6275069f),
                Diffuse = new Vector4(1, 0.9607844f, 0.8078432f, 1),
                Specular = new Vector4(1, 0.9607844f, 0.8078432f, 1),
            };
            Light1 = new DirectionalLight {
                Direction = new Vector3(0.7198464f, 0.3420201f, 0.6040227f),
                Diffuse = new Vector4(0.9647059f, 0.7607844f, 0.4078432f, 1),
                Specular = Vector4.One,
            };
            Light2 = new DirectionalLight {
                Direction = new Vector3(0.4545195f, -0.7660444f, 0.4545195f),
                Diffuse = new Vector4(0.3231373f, 0.3607844f, 0.3937255f, 1),
                Specular = new Vector4(0.3231373f, 0.3607844f, 0.3937255f, 1),
            };
            */
            this.AmbientColor = new Vector3(0.05333332f, 0.09882354f, 0.1819608f);
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
            this.ViewInverse = viewInverse;

            EyePosition = viewInverse.TranslationVector;
        }
        #endregion

        #region Input layouts
        private Dictionary<long, InputLayout> _InputLayouts = new Dictionary<long, InputLayout>();
        public InputLayout GetInputLayout(EffectPass pass) {

            var key = pass.NativePointer.ToInt64();
            if (_InputLayouts.ContainsKey(key))
                return _InputLayouts[key];

            var elements = Vertex3D.InputElements;
            var layout = new InputLayout(Device, pass.Description.Signature, elements);

            _InputLayouts.Add(key, layout);

            return layout;
        }
        #endregion

        protected override void Dispose(bool disposing) {
            foreach (var il in _InputLayouts.Values)
                il.Dispose();
            _InputLayouts.Clear();
            base.Dispose(disposing);
        }
    }
}
