using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Content {
    using SharpDX;
    using SharpDX.Direct3D11;

    public class BgMaterial : MaterialBase {

        #region Param map
        [Flags]
        public enum BgParameter : int {
            None = 0,
            Diffuse0 = 1 << 0,
            Specular0 = 1 << 1,
            Normal0 = 1 << 2,
            Diffuse1 = 1 << 3,
            Specular1 = 1 << 4,
            Normal1 = 1 << 5,
        }

        private static readonly IDictionary<BgParameter, string> TechniqueNames = new Dictionary<BgParameter, string> {
            { BgParameter.Normal0 | BgParameter.Diffuse0, "Single" },
            { BgParameter.Normal0 | BgParameter.Diffuse0 | BgParameter.Specular0, "SingleSpecular" },
            { BgParameter.Normal0 | BgParameter.Diffuse0 | BgParameter.Diffuse1 | BgParameter.Normal1, "Dual" },
            { BgParameter.Normal0 | BgParameter.Diffuse0 | BgParameter.Specular0 | BgParameter.Diffuse1 | BgParameter.Normal1 | BgParameter.Specular1, "DualSpecular" },
        };

        private static readonly IDictionary<uint, BgParameter> CharacterParameterMap = new Dictionary<uint, BgParameter> {
                // { 0xF0BAD919, "g_CameraParameter" },
                // { 0xC7DB2357, "g_InstancingData" },
                // { 0x64D12851, "g_MaterialParameter" },
                // { 0xEC4CCAA5, "g_InstanceData" },
                // { 0x1FDE0907, "g_PlateEadg" },
                // { 0xA9EAE61E, "g_EadgBias" },
                // { 0x73B31397, "g_WavingParam" },
                // { 0xCA0A5505, "g_RoofParameter" },
                // { 0x4D9F3ACD, "g_WetnessParameter" },
                // { 0xA9442826, "g_CommonParameter" },
                // { 0xA296769F, "g_AmbientParam" },
                // { 0xC8755A51, "g_BGAmbientParameter" },
                { 0x1BBC2F12, BgParameter.Specular0 }, // { 0x1BBC2F12, "g_SamplerSpecularMap0" },
                { 0x1E6FEF9C, BgParameter.Diffuse0 }, // { 0x1E6FEF9C, "g_SamplerColorMap0" },
                // { 0xBA8D7950, "g_SamplerFresnel" },
                // { 0xEBBB29BD, "g_SamplerGBuffer" },
                // { 0x23D0F850, "g_SamplerLightDiffuse" },
                // { 0x6C19ACA4, "g_SamplerLightSpecular" },
                // { 0x32667BD7, "g_SamplerOcclusion" },
                // { 0x9F467267, "g_SamplerDither" },
                { 0xAAB4D9E9, BgParameter.Normal0 }, // { 0xAAB4D9E9, "g_SamplerNormalMap0" },
                { 0x6CBB1F84, BgParameter.Specular1 }, // { 0x6CBB1F84, "g_SamplerSpecularMap1" },
                { 0x6968DF0A, BgParameter.Diffuse1 }, // { 0x6968DF0A, "g_SamplerColorMap1" },
                { 0xDDB3E97F, BgParameter.Normal1 }, // { 0xDDB3E97F, "g_SamplerNormalMap1" },
            };
        #endregion

        #region Properties
        public ShaderResourceView Diffuse0 { get; private set; }
        public ShaderResourceView Specular0 { get; private set; }
        public ShaderResourceView Normal0 { get; private set; }
        public ShaderResourceView Diffuse1 { get; private set; }
        public ShaderResourceView Specular1 { get; private set; }
        public ShaderResourceView Normal1 { get; private set; }

        public BgParameter Navin { get; private set; }

        public new Effects.BgEffect Effect { get { return (Effects.BgEffect)base.Effect; } }
        #endregion

        #region Constructor
        public BgMaterial(Engine engine, Material baseMaterial)
            : base(engine, baseMaterial) {

            Navin = BgParameter.None;

            foreach (var param in baseMaterial.TextureParameters) {
                var tex = baseMaterial.TexturesFiles[param.TextureIndex];
                BgParameter currentParam;
                if (!CharacterParameterMap.TryGetValue(param.ParameterId, out currentParam)) {
                    System.Diagnostics.Trace.WriteLine(string.Format("Unknown character parameter {0:X8} for texture '{1}' in material '{2}'.", param.ParameterId, tex.Path, baseMaterial.File.Path));
                    continue;
                }

                Navin |= currentParam;
                switch (currentParam) {
                    case BgParameter.Diffuse0:
                        Diffuse0 = Engine.TextureFactory.GetResource(tex);
                        break;
                    case BgParameter.Specular0:
                        Specular0 = Engine.TextureFactory.GetResource(tex);
                        break;
                    case BgParameter.Normal0:
                        Normal0 = Engine.TextureFactory.GetResource(tex);
                        break;
                    case BgParameter.Diffuse1:
                        Diffuse1 = Engine.TextureFactory.GetResource(tex);
                        break;
                    case BgParameter.Specular1:
                        Specular1 = Engine.TextureFactory.GetResource(tex);
                        break;
                    case BgParameter.Normal1:
                        Normal1 = Engine.TextureFactory.GetResource(tex);
                        break;
                }
            }

            CurrentTechniqueName = TechniqueNames[Navin];
        }
        #endregion

        #region Apply
        public override void Apply(Data.ParametersBase parameters) {
            Effect.Diffuse0 = this.Diffuse0;
            Effect.Specular0 = this.Specular0;
            Effect.Normal0 = this.Normal0;
            Effect.Diffuse1 = this.Diffuse1;
            Effect.Specular1 = this.Specular1;
            Effect.Normal1 = this.Normal1;
        }
        #endregion
    }
}
