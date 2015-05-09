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
                
                var eng = new SaintCoinach.Graphics.Viewer.Engine("Test");
                if (false) {
                    var exTerrains = new string[] {
                        "ex1/01_roc_r2/dun/r2d1/level/r2d1",    // Yes
                        "ex1/01_roc_r2/dun/r2d2/level/r2d2",
                        "ex1/01_roc_r2/evt/r2e1/level/r2e1",
                        "ex1/01_roc_r2/evt/r2e3/level/r2e3",
                        "ex1/01_roc_r2/evt/r2e4/level/r2e4",
                        "ex1/01_roc_r2/fld/r2f1/level/r2f1",
                        "ex1/01_roc_r2/twn/r2t1/level/r2t1",
                        "ex1/01_roc_r2/twn/r2t2/level/r2t2",
                        "ex1/01_roc_r2/twn/r2ti/level/r2ti",
                        "ex1/02_dra_d2/dun/d2d2/level/d2d2",    // Yes
                        "ex1/02_dra_d2/dun/d2d3/level/d2d3",
                        "ex1/02_dra_d2/fld/d2f1/level/d2f1",
                        "ex1/02_dra_d2/fld/d2f2/level/d2f2",    // Yes
                        "ex1/02_dra_d2/fld/d2f3/level/d2f3",    // Yes
                        "ex1/02_dra_d2/fld/d2fa/level/d2fa",
                        "ex1/03_abr_a2/dun/a2d2/level/a2d2",
                        "ex1/03_abr_a2/dun/a2d3/level/a2d3",
                        "ex1/03_abr_a2/fld/a2f1/level/a2f1",    // Yes
                        "ex1/03_abr_a2/fld/a2f2/level/a2f2",
                        "ex1/04_sea_s2/pvp/s2p1/level/s2p1",
                    };
                    var terr = new SaintCoinach.Graphics.Territory(packs, "LaLa", exTerrains[18]);
                    if (terr.Terrain == null && terr.LgbFiles.Count() == 0)
                        throw new KeyNotFoundException();
                    var engTerr = new SaintCoinach.Graphics.Viewer.Content.ContentTerritory(eng, terr);
                    eng.Components.Add(engTerr);
                }
                if (true) {
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
                    
                    var eq = (SaintCoinach.Xiv.Items.Equipment)arr.GameData.GetSheet<SaintCoinach.Xiv.Item>().First(i => i.Name == "Lionsmane Cuirass");
                    SaintCoinach.Graphics.ImcVariant imcVar;
                    var mdlDef = eq.GetModel(101, out imcVar);
                    var mdlVar = new SaintCoinach.Graphics.ModelVariantIdentifier {
                        ImcVariant = imcVar,
                        StainKey = 14

                    };

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

                    var engMdl = new SaintCoinach.Graphics.Viewer.Content.ContentModel(eng, mdlVar, mdlDef, SaintCoinach.Graphics.ModelQuality.High);
                    eng.Components.Add(engMdl);
                }

                eng.Run();
            }
        }
    }
}
