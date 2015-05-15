using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Content {
    using SharpDX;
    using SharpDX.Direct3D11;

    public class HairMaterial : MaterialBase {

        #region Param map
        [Flags]
        public enum HairParameter : int {
            None = 0,
            Normal = 1 << 0,
            Mask = 1 << 1,
        }

        private static readonly IDictionary<uint, HairParameter> CharacterParameterMap = new Dictionary<uint, HairParameter> {
                { 0x0C5EC1F1, HairParameter.Normal }, // { 0x0C5EC1F1, "g_SamplerNormal" },
                { 0x8A4E82B6, HairParameter.Mask }, // { 0x8A4E82B6, "g_SamplerMask" },
            };
        #endregion

        #region Properties
        public ShaderResourceView Normal { get; private set; }
        public ShaderResourceView Mask { get; private set; }

        public HairParameter Navin { get; private set; }

        public new Effects.HairEffect Effect { get { return (Effects.HairEffect)base.Effect; } }
        #endregion

        #region Constructor
        public HairMaterial(Engine engine, Material baseMaterial)
            : base(engine, baseMaterial) {

            Navin = HairParameter.None;

            foreach (var param in baseMaterial.TextureParameters) {
                var tex = baseMaterial.TexturesFiles[param.TextureIndex];
                HairParameter currentParam;
                if (!CharacterParameterMap.TryGetValue(param.ParameterId, out currentParam)) {
                    System.Diagnostics.Trace.WriteLine(string.Format("Unknown character parameter {0:X8} for texture '{1}' in material '{2}'.", param.ParameterId, tex.Path, baseMaterial.File.Path));
                    continue;
                }

                Navin |= currentParam;
                switch (currentParam) {
                    case HairParameter.Normal:
                        Normal = Engine.TextureFactory.GetResource(tex);
                        break;
                    case HairParameter.Mask:
                        Mask = Engine.TextureFactory.GetResource(tex);
                        break;
                }
            }

            CurrentTechniqueName = "Hair";
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
