using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public static partial class Utilities {
        public static class Effect {
            public static string GetTechniqueName(Assets.MaterialVersion material) {
                switch (material.Shader) {
                    case "character.shpk":
                        return GetCharacterTechniqueName(material);
                    case "bg.shpk":
                        return GetBgTechniqueName(material);
                }
                throw new NotSupportedException();
            }

            #region Bg

            #region Parameter map
            public static readonly IReadOnlyDictionary<uint, string> BgParameterMap = new ReadOnlyDictionary<uint, string>(new Dictionary<uint, string>{
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
                { 0x1BBC2F12, "Specular0" }, // { 0x1BBC2F12, "g_SamplerSpecularMap0" },
                { 0x1E6FEF9C, "Diffuse0" }, // { 0x1E6FEF9C, "g_SamplerColorMap0" },
                // { 0xBA8D7950, "g_SamplerFresnel" },
                // { 0xEBBB29BD, "g_SamplerGBuffer" },
                // { 0x23D0F850, "g_SamplerLightDiffuse" },
                // { 0x6C19ACA4, "g_SamplerLightSpecular" },
                // { 0x32667BD7, "g_SamplerOcclusion" },
                // { 0x9F467267, "g_SamplerDither" },
                { 0xAAB4D9E9, "Normal0" }, // { 0xAAB4D9E9, "g_SamplerNormalMap0" },
                { 0x6CBB1F84, "Specular1" }, // { 0x6CBB1F84, "g_SamplerSpecularMap1" },
                { 0x6968DF0A, "Diffuse1" }, // { 0x6968DF0A, "g_SamplerColorMap1" },
                { 0xDDB3E97F, "Normal1" }, // { 0xDDB3E97F, "g_SamplerNormalMap1" },
            });
            #endregion

            #region Technique map
            static readonly Dictionary<uint[], string> BgTechniqueMap = new Dictionary<uint[], string> {
                { new uint[] { 0x1E6FEF9C, 0xAAB4D9E9, }, "Single" },
                { new uint[] { 0x1E6FEF9C, 0xAAB4D9E9, 0x1BBC2F12 }, "SingleSpecular" },
                { new uint[] { 0x1E6FEF9C, 0xAAB4D9E9, 0x6968DF0A, 0xDDB3E97F }, "Dual" },
                { new uint[] { 0x1E6FEF9C, 0xAAB4D9E9, 0x1BBC2F12, 0x6968DF0A, 0xDDB3E97F, 0x6CBB1F84 }, "DualSpecular" },
            };
            #endregion

            static string GetBgTechniqueName(Assets.MaterialVersion material) {
                var idFromMat = material.ParameterMappings.Select(_ => _.Id).ToArray();
                var names = idFromMat.ToDictionary(_ => _, _ => BgParameterMap.Where(__ => __.Key == _).Select(__ => __.Value).FirstOrDefault());

                var techRes = BgTechniqueMap.Where(_ => EqualContent(_.Key, idFromMat));
                if (!techRes.Any()) {
                    throw new NotSupportedException();
                }

                return techRes.First().Value;
            }

            #endregion

            #region Character

            #region Parameter map
            public static readonly IReadOnlyDictionary<uint, string> CharacterParameterMap = new ReadOnlyDictionary<uint, string>(new Dictionary<uint, string> {
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
                { 0x0C5EC1F1, "Normal" }, // { 0x0C5EC1F1, "g_SamplerNormal" },
                // { 0x565F8FD8, "g_SamplerIndex" },
                { 0x2005679F, "Table" }, // { 0x2005679F, "g_SamplerTable" },
                // { 0x92F03E53, "g_SamplerTileNormal" },
                // { 0xEBBB29BD, "g_SamplerGBuffer" },
                // { 0x23D0F850, "g_SamplerLightDiffuse" },
                // { 0x6C19ACA4, "g_SamplerLightSpecular" },
                { 0x8A4E82B6, "Mask" }, // { 0x8A4E82B6, "g_SamplerMask" },
                // { 0x32667BD7, "g_SamplerOcclusion" },
                // { 0x87F6474D, "g_SamplerReflection" },
                // { 0x29156A85, "g_SamplerTileDiffuse" },
                // { 0x9F467267, "g_SamplerDither" },
                { 0x115306BE, "Diffuse" }, // { 0x115306BE, "g_SamplerDiffuse" },
                { 0x2B99E025, "Specular" }, // { 0x2B99E025, "g_SamplerSpecular" },
                // { 0x0237CB94, "g_SamplerDecal" },

                // It's just one pixel, adding it as mask here, but going to be ignored
                // See: chara/monster/m8014/obj/body/b0001/model/m8014b0001.mdl
                //      chara/monster/m9999/obj/body/b0001/model/m9999b0001.mdl
                { 0xCC28F4AD, "Mask" },
            });
            #endregion

            #region Technique map
            static readonly Dictionary<uint[], string> CharacterTechniqueMap = new Dictionary<uint[], string> {
                { new uint[] { 0x115306BE, 0x0C5EC1F1, 0xCC28F4AD }, "Diffuse" },
                { new uint[] { 0x115306BE, 0x0C5EC1F1, 0x2B99E025 }, "DiffuseSpecular" },
                { new uint[] { 0x115306BE, 0x0C5EC1F1, 0x2B99E025, 0x2005679F }, "DiffuseSpecularTable" },
                { new uint[] { 0x0C5EC1F1, 0x8A4E82B6, 0x2005679F }, "MaskTable" },
            };
            #endregion

            static string GetCharacterTechniqueName(Assets.MaterialVersion material) {
                var idFromMat = material.ParameterMappings.Select(_ => _.Id).ToArray();

                var techRes = CharacterTechniqueMap.Where(_ => EqualContent(_.Key, idFromMat));
                if (!techRes.Any())
                    throw new NotSupportedException();

                return techRes.First().Value;
            }
            #endregion

            #region Helpers
            static bool EqualContent<T>(T[] a1, T[] a2) {
                var a1Ea2 = a1.Except(a2);
                var a2Ea1 = a2.Except(a1);

                return !a1Ea2.Any() && !a2Ea1.Any();
            }
            #endregion
        }
    }
}
