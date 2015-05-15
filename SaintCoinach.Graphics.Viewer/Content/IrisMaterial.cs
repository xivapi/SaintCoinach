using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Content {
    using SharpDX;
    using SharpDX.Direct3D11;

    public class IrisMaterial : MaterialBase {

        #region Param map
        [Flags]
        public enum IrisParameter : int {
            None = 0,
            Normal = 1 << 0,
            Mask = 1 << 1,
        }

        private static readonly IDictionary<uint, IrisParameter> CharacterParameterMap = new Dictionary<uint, IrisParameter> {
                { 0x0C5EC1F1, IrisParameter.Normal }, // { 0x0C5EC1F1, "g_SamplerNormal" },
                // { 0xFEA0F3D2, "g_SamplerCatchlight" },
                // { 0xEBBB29BD, "g_SamplerGBuffer" },
                // { 0x23D0F850, "g_SamplerLightDiffuse" },
                // { 0x6C19ACA4, "g_SamplerLightSpecular" },
                { 0x8A4E82B6, IrisParameter.Mask }, // { 0x8A4E82B6, "g_SamplerMask" },
                // { 0x32667BD7, "g_SamplerOcclusion" },
                // { 0x87F6474D, "g_SamplerReflection" },
                // { 0x9F467267, "g_SamplerDither" },
            };
        #endregion

        #region Properties
        public ShaderResourceView Normal { get; private set; }
        public ShaderResourceView Mask { get; private set; }

        public IrisParameter Navin { get; private set; }

        public new Effects.IrisEffect Effect { get { return (Effects.IrisEffect)base.Effect; } }
        #endregion

        #region Constructor
        public IrisMaterial(Engine engine, Material baseMaterial)
            : base(engine, baseMaterial) {

            Navin = IrisParameter.None;

            foreach (var param in baseMaterial.TextureParameters) {
                var tex = baseMaterial.TexturesFiles[param.TextureIndex];
                IrisParameter currentParam;
                if (!CharacterParameterMap.TryGetValue(param.ParameterId, out currentParam)) {
                    System.Diagnostics.Trace.WriteLine(string.Format("Unknown character parameter {0:X8} for texture '{1}' in material '{2}'.", param.ParameterId, tex.Path, baseMaterial.File.Path));
                    continue;
                }

                Navin |= currentParam;
                switch (currentParam) {
                    case IrisParameter.Normal:
                        Normal = Engine.TextureFactory.GetResource(tex);
                        break;
                    case IrisParameter.Mask:
                        Mask = Engine.TextureFactory.GetResource(tex);
                        break;
                }
            }

            CurrentTechniqueName = "Iris";
        }
        #endregion

        #region Apply
        public override void Apply(Data.ParametersBase parameters) {
            Effect.Normal = this.Normal;
            Effect.Mask = this.Mask;

            Effect.CustomizeParameters = parameters.GetValueOrDefault(Data.CustomizeParameters.CustomizeParametersKey, Data.CustomizeParameters.Default);
        }
        #endregion
    }
}
