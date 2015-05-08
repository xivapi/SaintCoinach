using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest {
    class Program {
        static void Main(string[] args) {
            var arr = new SaintCoinach.ARealmReversed(@"C:\Program Files (x86)\SquareEnix\FINAL FANTASY XIV - A Realm Reborn", SaintCoinach.Ex.Language.English);
            var packs = arr.Packs;

            {
                /*
                var imc = new SaintCoinach.Graphics.ImcFile(packs.GetFile("chara/monster/m0099/obj/body/b0001/b0001.imc"));
                var mdlFile = (SaintCoinach.Graphics.ModelFile)packs.GetFile("chara/monster/m0099/obj/body/b0001/model/m0099b0001.mdl");
                var mdlDef = mdlFile.GetModelDefinition();
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
                var imcVar = imc.GetVariant(1, 2);*/
                
                var eq = (SaintCoinach.Xiv.Items.Equipment)arr.GameData.GetSheet<SaintCoinach.Xiv.Item>().First(i => i.Name == "Augmented Ironworks Armor of Fending");
                SaintCoinach.Graphics.ImcVariant imcVar;
                var mdlDef = eq.GetModel(101, out imcVar);
                var mdlVar = new SaintCoinach.Graphics.ModelVariantIdentifier {
                    ImcVariant = imcVar,
                    StainKey = 14

                };

                var eng = new SaintCoinach.Graphics.Viewer.Engine("Test");
                var engMdl = new SaintCoinach.Graphics.Viewer.Content.ContentModel(eng, mdlVar, mdlDef, SaintCoinach.Graphics.ModelQuality.High);
                eng.Components.Add(engMdl);
                eng.Run();
            }
        }
    }
}
