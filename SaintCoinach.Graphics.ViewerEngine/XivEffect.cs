using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics {
    public abstract class XivEffect : Effect {
        #region Effect variables
        private EffectVectorVariable _EyePositionVar;
        private EffectPointLightVariable _Light0Var;
        private EffectPointLightVariable _Light1Var;
        private EffectPointLightVariable _Light2Var;
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
        public PointLight Light0 {
            get { return _Light0Var.Get(); }
            set { _Light0Var.Set(value); }
        }
        public PointLight Light1 {
            get { return _Light1Var.Get(); }
            set { _Light1Var.Set(value); }
        }
        public PointLight Light2 {
            get { return _Light2Var.Get(); }
            set { _Light2Var.Set(value); }
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
        public abstract Type RequiredVertexType { get; }
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
            _Light0Var = new EffectPointLightVariable(GetVariableByName("Light0"));
            _Light1Var = new EffectPointLightVariable(GetVariableByName("Light1"));
            _Light2Var = new EffectPointLightVariable(GetVariableByName("Light2"));
            _EmissiveColorVar = GetVariableByName("EmissiveColor").AsVector();
            _AmbientColorVar = GetVariableByName("AmbientColor").AsVector();
            _WorldVar = GetVariableByName("World").AsMatrix();
            _WorldInverseTransposeVar = GetVariableByName("WorldInverseTranspose").AsMatrix();
            _WorldViewProjectionVar = GetVariableByName("WorldViewProj").AsMatrix();

            // Key light.
            Light0 = new PointLight {
                Direction = new Vector3(0.5265408f, -0.5735765f, 0.6275069f),
                Diffuse = new Vector3(1, 1, 1),
                Specular = new Vector3(1, 1, 1),
            };

            // Fill light.
            Light1 = new PointLight {
                Direction = new Vector3(-0.7198464f, 0.3420201f, -0.6040227f),
                Diffuse = new Vector3(0.7607844f, 0.7607844f, 0.7607844f),
                Specular = Vector3.Zero,
            };

            // Back light.
            Light2 = new PointLight {
                Direction = new Vector3(-0.4545195f, -0.7660444f, -0.4545195f),
                Diffuse = new Vector3(0.3937255f, 0.3937255f, 0.3937255f),
                Specular = new Vector3(0.3937255f, 0.3937255f, 0.3937255f),
            };

            //EmissiveColor = new Vector3(0.2f, 0.2f, 0.2f);
            AmbientColor = 3 * new Vector3(0.09882354f, 0.09882354f, 0.09882354f);
            EmissiveColor = AmbientColor;

            /*
            // Key light.
            Light0 = new PointLight {
                Direction = new Vector3(-0.5265408f, -0.5735765f, -0.6275069f),
                Diffuse = Vector3.One,
                Specular = new Vector3(.75f, .75f, .75f),
            };

            // Fill light.
            Light1 = new PointLight {
                Direction = new Vector3(0.7198464f, 0.3420201f, 0.6040227f),
                Diffuse = Vector3.One,
                Specular = Vector3.Zero
            };

            // Back light.
            Light2 = new PointLight {
                Direction = new Vector3(0.4545195f, -0.7660444f, 0.4545195f),
                Diffuse = Vector3.One,
                Specular = new Vector3(0.3607844f, 0.3607844f, 0.3607844f)
            };

            //EmissiveColor = new Vector3(0.2f, 0.2f, 0.2f);
            AmbientColor = new Vector3(0.1819608f, 0.1819608f, 0.1819608f);*/
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

        #region Input layouts
        private Dictionary<Tuple<Type, int>, InputLayout> _InputLayouts = new Dictionary<Tuple<Type, int>, InputLayout>();
        public InputLayout GetInputLayout<T>(EffectPass pass) {
            var t = typeof(T);
            return GetInputLayout(pass, t);
        }
        public InputLayout GetInputLayout(EffectPass pass, Type type) {

            var key = Tuple.Create<Type, int>(type, pass.NativePointer.ToInt32());
            if (_InputLayouts.ContainsKey(key))
                return _InputLayouts[key];

            PropertyInfo property = type.GetProperty("InputElements", BindingFlags.Static | BindingFlags.Public);
            if (property == null) {
                var allProperties = type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                var arrType = typeof(InputElement[]);
                var matches = allProperties.Where(_ => arrType.IsAssignableFrom(_.PropertyType));
                if (!matches.Any())
                    throw new NotSupportedException("Unable to get input elements of target type.");

                property = matches.First();
            }

            var elements = (InputElement[])property.GetValue(null);
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
