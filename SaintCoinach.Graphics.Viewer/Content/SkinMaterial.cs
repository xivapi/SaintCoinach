using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Content {
    using SharpDX;
    using SharpDX.Direct3D11;

    public class SkinMaterial : MaterialBase {

        #region Param map
        [Flags]
        public enum SkinParameter : int {
            None = 0,
            Diffuse = 1 << 0,
            Normal = 1 << 1,
            Mask = 1 << 2,
        }

        private static readonly IDictionary<uint, SkinParameter> CharacterParameterMap = new Dictionary<uint, SkinParameter> {
                { 0x0C5EC1F1, SkinParameter.Normal }, // { 0x0C5EC1F1, "g_SamplerNormal" },
                { 0x8A4E82B6, SkinParameter.Mask }, // { 0x8A4E82B6, "g_SamplerMask" },
                { 0x115306BE, SkinParameter.Diffuse }, // { 0x115306BE, "g_SamplerDiffuse" },
            };
        #endregion

        #region Properties
        public ShaderResourceView Diffuse { get; private set; }
        public ShaderResourceView Normal { get; private set; }
        public ShaderResourceView Mask { get; private set; }

        public SkinParameter Navin { get; private set; }

        public new Effects.SkinEffect Effect { get { return (Effects.SkinEffect)base.Effect; } }
        #endregion

        #region Constructor
        public SkinMaterial(Engine engine, Material baseMaterial)
            : base(engine, baseMaterial) {

            Navin = SkinParameter.None;

            foreach (var param in baseMaterial.TextureParameters) {
                var tex = baseMaterial.TexturesFiles[param.TextureIndex];
                SkinParameter currentParam;
                if (!CharacterParameterMap.TryGetValue(param.ParameterId, out currentParam)) {
                    System.Diagnostics.Trace.WriteLine(string.Format("Unknown character parameter {0:X8} for texture '{1}' in material '{2}'.", param.ParameterId, tex.Path, baseMaterial.File.Path));
                    continue;
                }

                Navin |= currentParam;
                switch (currentParam) {
                    case SkinParameter.Diffuse:
                        Diffuse = Engine.TextureFactory.GetResource(tex);
                        break;
                    case SkinParameter.Normal:
                        Normal = Engine.TextureFactory.GetResource(tex);
                        break;
                    case SkinParameter.Mask:
                        Mask = Engine.TextureFactory.GetResource(tex);
                        break;
                }
            }

            CurrentTechniqueName = "Skin";
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
