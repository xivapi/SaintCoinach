using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest {
    class Program {
        static void Main(string[] args) {
            var arr = new SaintCoinach.ARealmReversed(@"C:\Program Files (x86)\SquareEnix\FINAL FANTASY XIV - A Realm Reborn", SaintCoinach.Ex.Language.English);
            //var arr = new SaintCoinach.ARealmReversed(@"C:\XIVVersions\HW-Bench", SaintCoinach.Ex.Language.English);
            var packs = arr.Packs;

            {
                // 95   Garuda
                // 96   Ifrit
                // 97   Levi
                // 98   Titan
                // 99   Ultima Weapon
                // 100  Sleipnir
                // 101  Shiva
                // 102  Vishap
                // 104  Ghost snake thing ? (it's all transparent)
                // 105  ADS
                // 106  Rock (from Garuda)
                // 107  Proto-Ultima arm unit
                /*
                var __ = new object[] {
                    96,
                    1
                };
                var imcPath = string.Format("chara/monster/m{0:D4}/obj/body/b{1:D4}/b{1:D4}.imc", __);
                var mdlPath = string.Format("chara/monster/m{0:D4}/obj/body/b{1:D4}/model/m{0:D4}b{1:D4}.mdl", __);
                var imc = new SaintCoinach.Graphics.ImcFile(packs.GetFile(imcPath));
                var mdlFile = (SaintCoinach.Graphics.ModelFile)packs.GetFile(mdlPath);
                var mdlDef = mdlFile.GetModelDefinition();
                Console.WriteLine(imc.Count);
                var imcVar = imc.GetVariant(1);
                var mdlVar = imcVar;
                */
                /*
                var imc = new SaintCoinach.Graphics.ImcFile(packs.GetFile("chara/equipment/e0124/e0124.imc"));
                var mdlFile = (SaintCoinach.Graphics.ModelFile)packs.GetFile("chara/equipment/e0124/model/c0201e0124_top.mdl");
                var mdlDef = mdlFile.GetModelDefinition();
                for (var j = 0; j <= 2; ++j) {

                    for (byte i = 0; i < 5; ++i) {
                        var v = imc.GetVariant(i, j);
                        Console.WriteLine("IMC#{4}-{0}: {1:X4} / {2:X2} / {3:X2}", i, v.PartVisibilityMask, v.Unknown3, v.Unknown4, j);
                    }
                }
                var imcVar = imc.GetVariant(1, 1);
                */
                /*
                var imc = new SaintCoinach.Graphics.ImcFile(packs.GetFile("chara/equipment/e0124/e0124.imc"));
                var mdlFile = (SaintCoinach.Graphics.ModelFile)packs.GetFile("chara/equipment/e0124/model/c0101e0124_top.mdl");
                var mdlDef = mdlFile.GetModelDefinition();
                var imcVar = imc.GetVariant(1, 2);
                 */
                /*
                var eq = (SaintCoinach.Xiv.Items.Equipment)arr.GameData.GetSheet<SaintCoinach.Xiv.Item>().First(i => i.Name == "Lionliege Cuirass");
                SaintCoinach.Graphics.ImcVariant imcVar;
                var mdlDef = eq.GetModel(101, out imcVar);
                var mdlVar = new SaintCoinach.Graphics.ModelVariantIdentifier {
                    ImcVariant = imcVar,
                    StainKey = 14

                };*/

                /*
                var mdlFile = (SaintCoinach.Graphics.ModelFile)packs.GetFile(@"chara/human/c0201/obj/hair/h0004/model/c0201h0004_hir.mdl");
                var mdlDef = mdlFile.GetModelDefinition();
                var mdlVar = SaintCoinach.Graphics.ImcVariant.Default;
                */
                /*
                var mdlFile = (SaintCoinach.Graphics.ModelFile)packs.GetFile(@"chara/human/c0201/obj/face/f0003/model/c0201f0003_fac.mdl");
                var mdlDef = mdlFile.GetModelDefinition();
                var mdlVar = SaintCoinach.Graphics.ImcVariant.Default;
                 */

                var mdlFile = (SaintCoinach.Graphics.ModelFile)packs.GetFile(@"bg/ffxiv/sea_s1/fld/s1f1/bgplate/0088.mdl");
                var mdlDef = mdlFile.GetModelDefinition();
                var mdlVar = SaintCoinach.Graphics.ImcVariant.Default;

                var eng = new SaintCoinach.Graphics.Viewer.Engine("Test");
                var engMdl = new SaintCoinach.Graphics.Viewer.Content.ContentModel(eng, mdlVar, mdlDef, SaintCoinach.Graphics.ModelQuality.High);
                eng.Components.Add(engMdl);
                eng.Run();
            }
        }
    }
}
