using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Content {
    using SharpDX;
    using SharpDX.Direct3D11;

    public class BgColorChangeMaterial : MaterialBase {
        public const string ColorParameterKey = "BgColorChangeColor";
        public static Vector4 DefaultColor = new Vector4(228 / 255f, 223 / 255f, 208 / 208f, 1f);

        #region Param map
        [Flags]
        public enum BgColorChangeParameter : int {
            None = 0,
            ColorMap0 = 1 << 0,
            SpecularMap0 = 1 << 1,
            NormalMap0 = 1 << 2,
        }

        private static readonly IDictionary<uint, BgColorChangeParameter> CharacterParameterMap = new Dictionary<uint, BgColorChangeParameter> {
            // { 0xF0BAD919, "g_CameraParameter" },
            // { 0xC7DB2357, "g_InstancingData" },
            // { 0x64D12851, "g_MaterialParameter" },
            // { 0xEC4CCAA5, "g_InstanceData" },
            // { 0xCA0A5505, "g_RoofParameter" },
            // { 0xA9442826, "g_CommonParameter" },
            // { 0xA296769F, "g_AmbientParam" },
            // { 0xC8755A51, "g_BGAmbientParameter" },
            // { 0x4D9F3ACD, "g_WetnessParameter" },
            { 0xAAB4D9E9, BgColorChangeParameter.NormalMap0 }, // { 0xAAB4D9E9, "g_SamplerNormalMap0" },
            { 0x1BBC2F12, BgColorChangeParameter.SpecularMap0 }, // { 0x1BBC2F12, "g_SamplerSpecularMap0" },
            { 0x1E6FEF9C, BgColorChangeParameter.ColorMap0 }, // { 0x1E6FEF9C, "g_SamplerColorMap0" },
            // { 0xBA8D7950, "g_SamplerFresnel" },
            // { 0xEBBB29BD, "g_SamplerGBuffer" },
            // { 0x23D0F850, "g_SamplerLightDiffuse" },
            // { 0x6C19ACA4, "g_SamplerLightSpecular" },
            // { 0x32667BD7, "g_SamplerOcclusion" },
            // { 0x9F467267, "g_SamplerDither" },
            };
        #endregion

        #region Properties
        public ShaderResourceView ColorMap0 { get; private set; }
        public ShaderResourceView SpecularMap0 { get; private set; }
        public ShaderResourceView NormalMap0 { get; private set; }

        public BgColorChangeParameter Navin { get; private set; }

        public new Effects.BgColorChangeEffect Effect { get { return (Effects.BgColorChangeEffect)base.Effect; } }
        #endregion

        #region Constructor
        public BgColorChangeMaterial(Engine engine, Material baseMaterial)
            : base(engine, baseMaterial) {

            Navin = BgColorChangeParameter.None;

            foreach (var param in baseMaterial.TextureParameters) {
                var tex = baseMaterial.TexturesFiles[param.TextureIndex];
                BgColorChangeParameter currentParam;
                if (!CharacterParameterMap.TryGetValue(param.ParameterId, out currentParam)) {
                    System.Diagnostics.Trace.WriteLine(string.Format("Unknown character parameter {0:X8} for texture '{1}' in material '{2}'.", param.ParameterId, tex.Path, baseMaterial.File.Path));
                    continue;
                }

                Navin |= currentParam;
                switch (currentParam) {
                    case BgColorChangeParameter.ColorMap0:
                        ColorMap0 = Engine.TextureFactory.GetResource(tex);
                        break;
                    case BgColorChangeParameter.SpecularMap0:
                        SpecularMap0 = Engine.TextureFactory.GetResource(tex);
                        break;
                    case BgColorChangeParameter.NormalMap0:
                        NormalMap0 = Engine.TextureFactory.GetResource(tex);
                        break;
                }
            }

            CurrentTechniqueName = "BgColorChange"; // TechniqueNames[Navin];
        }
        #endregion

        #region Apply
        public override void Apply(Data.ParametersBase parameters) {
            Effect.ColorMap0 = this.ColorMap0;
            Effect.SpecularMap0 = this.SpecularMap0;
            Effect.NormalMap0 = this.NormalMap0;
            Effect.Color = parameters.GetValueOrDefault(ColorParameterKey, DefaultColor);
        }
        #endregion
    }
}
