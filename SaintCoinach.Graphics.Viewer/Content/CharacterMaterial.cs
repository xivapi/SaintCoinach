using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Content {
    using SharpDX;
    using SharpDX.Direct3D11;

    public class CharacterMaterial : MaterialBase {

        #region Param map
        [Flags]
        public enum CharacterParameter : int {
            None        = 0,
            Diffuse     = 1 << 0,
            Specular    = 1 << 1,
            Normal      = 1 << 2,
            Mask        = 1 << 3,
            Table       = 1 << 4,
        }
        
        private static readonly IDictionary<CharacterParameter, string> TechniqueNames = new Dictionary<CharacterParameter, string> {
            { CharacterParameter.Normal | CharacterParameter.Diffuse, "Diffuse" },
            { CharacterParameter.Normal | CharacterParameter.Diffuse | CharacterParameter.Specular, "DiffuseSpecular" },
            { CharacterParameter.Normal | CharacterParameter.Diffuse | CharacterParameter.Specular | CharacterParameter.Table, "DiffuseSpecularTable" },
            { CharacterParameter.Normal | CharacterParameter.Diffuse | CharacterParameter.Table, "DiffuseTable" },
            { CharacterParameter.Normal | CharacterParameter.Mask | CharacterParameter.Table, "MaskTable" },
            { CharacterParameter.Normal | CharacterParameter.Mask, "Mask" },
        };

        private static readonly IDictionary<uint, CharacterParameter> CharacterParameterMap = new Dictionary<uint, CharacterParameter> {
                // { 0xF0BAD919, "g_CameraParameter" },
                // { 0x76BB3DC0, "g_WorldViewMatrix" },
                // { 0x20A30B34, "g_InstanceParameter" },
                // { 0x88AA546A, "g_JointMatrixArray" },
                // { 0x64D12851, "g_MaterialParameter" },
                // { 0x3D086484, "g_SceneParameter" },
                // { 0xA9442826, "g_CommonParameter" },
                // { 0xA296769F, "g_AmbientParam" },
                // { 0x77F6BFB3, "g_MaterialParameterDynamic" },
                // { 0x5B0F708C, "g_DecalColor" },
                // { 0xEF4E7491, "g_LightDirection" },
                { 0x0C5EC1F1, CharacterParameter.Normal }, // { 0x0C5EC1F1, "g_SamplerNormal" },
                // { 0x565F8FD8, "g_SamplerIndex" },
                { 0x2005679F, CharacterParameter.Table }, // { 0x2005679F, "g_SamplerTable" },
                // { 0x92F03E53, "g_SamplerTileNormal" },
                // { 0xEBBB29BD, "g_SamplerGBuffer" },
                // { 0x23D0F850, "g_SamplerLightDiffuse" },
                // { 0x6C19ACA4, "g_SamplerLightSpecular" },
                { 0x8A4E82B6, CharacterParameter.Mask }, // { 0x8A4E82B6, "g_SamplerMask" },
                // { 0x32667BD7, "g_SamplerOcclusion" },
                // { 0x87F6474D, "g_SamplerReflection" },
                // { 0x29156A85, "g_SamplerTileDiffuse" },
                // { 0x9F467267, "g_SamplerDither" },
                { 0x115306BE, CharacterParameter.Diffuse }, // { 0x115306BE, "g_SamplerDiffuse" },
                { 0x2B99E025, CharacterParameter.Specular }, // { 0x2B99E025, "g_SamplerSpecular" },
                // { 0x0237CB94, "g_SamplerDecal" },
            };
        #endregion

        #region Properties
        public ShaderResourceView Diffuse { get; private set; }
        public ShaderResourceView Specular { get; private set; }
        public ShaderResourceView Normal { get; private set; }
        public ShaderResourceView Mask { get; private set; }
        public ShaderResourceView Table { get; private set; }

        public CharacterParameter Navin { get; private set; }

        public new Effects.CharacterEffect Effect { get { return (Effects.CharacterEffect)base.Effect; } }
        #endregion

        #region Constructor
        public CharacterMaterial(Engine engine, Material baseMaterial)
            : base(engine, baseMaterial) {

            Navin = CharacterParameter.None;

            foreach (var param in baseMaterial.TextureParameters) {
                if (param.TextureIndex == byte.MaxValue)
                    continue;   // TODO: See if this may actually refer to dummy texture (even though others just have a dummy.tex entry)
                var tex = baseMaterial.TexturesFiles[param.TextureIndex];
                CharacterParameter currentParam;
                if (!CharacterParameterMap.TryGetValue(param.ParameterId, out currentParam)) {
                    System.Diagnostics.Trace.WriteLine(string.Format("Unknown character parameter {0:X8} for texture '{1}' in material '{2}'.", param.ParameterId, tex.Path, baseMaterial.File.Path));
                    continue;
                }

                Navin |= currentParam;
                switch (currentParam) {
                    case CharacterParameter.Diffuse:
                        Diffuse = Engine.TextureFactory.GetResource(tex);
                        break;
                    case CharacterParameter.Specular:
                        Specular = Engine.TextureFactory.GetResource(tex);
                        break;
                    case CharacterParameter.Normal:
                        Normal = Engine.TextureFactory.GetResource(tex);
                        break;
                    case CharacterParameter.Mask:
                        Mask = Engine.TextureFactory.GetResource(tex);
                        break;
                    case CharacterParameter.Table:
                        Table = Engine.TextureFactory.GetResource(tex);
                        break;
                }
            }

            CurrentTechniqueName = TechniqueNames[Navin];
        }
        #endregion

        #region Apply
        public override void Apply(Data.ParametersBase parameters) {
            Effect.Diffuse = this.Diffuse;
            Effect.Specular = this.Specular;
            Effect.Normal = this.Normal;
            Effect.Mask = this.Mask;
            Effect.Table = this.Table;
        }
        #endregion
    }
}
