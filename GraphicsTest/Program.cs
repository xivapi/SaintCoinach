using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest {
    class Program {
        static void Main(string[] args) {
            var packs = new SaintCoinach.IO.PackCollection(@"C:\Program Files (x86)\SquareEnix\FINAL FANTASY XIV - A Realm Reborn\game\sqpack");
            {
                // m0099 is Ultima Weapon
                var imc = new SaintCoinach.Graphics.ImcFile(packs.GetFile("chara/monster/m0099/obj/body/b0001/b0001.imc"));
                var mdlFile = (SaintCoinach.Graphics.ModelFile)packs.GetFile("chara/monster/m0099/obj/body/b0001/model/m0099b0001.mdl");
                var def = new SaintCoinach.Graphics.ModelDefinition(mdlFile);
                var mdl = def.GetModel(0);
                var mdl2 = def.GetModel(1);

                // Variant at 4 seems to be missing wings
                var imcVar = imc.GetVariant(4);

                var materials = new SaintCoinach.Graphics.Material[mdl.Meshes.Length];
                for (var i = 0; i < mdl.Meshes.Length; ++i) {
                    materials[i] = mdl.Meshes[i].Material.Get(imcVar);

                    for (var j = 0; j < mdl.Meshes[i].Parts.Length; ++j) {
                        // TODO: Confirm this to actually be true.
                        var result = ((imcVar.PartVisibilityMask & mdl.Meshes[i].Parts[j].Header.VisibilityMask) == mdl.Meshes[i].Parts[j].Header.VisibilityMask);
                        Console.WriteLine("Mesh[{0}].Part[{1}].Visible = {2}", i, j, result);
                    }
                }


                System.Diagnostics.Debugger.Break();
            }
            {
                // e0124 is PLD PvP armours
                var imc = new SaintCoinach.Graphics.ImcFile(packs.GetFile("chara/equipment/e0124/e0124.imc"));
                var mdlFile = (SaintCoinach.Graphics.ModelFile)packs.GetFile("chara/equipment/e0124/model/c0101e0124_top.mdl");
                var def = new SaintCoinach.Graphics.ModelDefinition(mdlFile);
                var mdl = def.GetModel(0);

                // Variant at 1 should have no mane and missing a bit on the belt.
                var imcVar = imc.GetVariant(1, 1);

                var materials = new SaintCoinach.Graphics.Material[mdl.Meshes.Length];
                for (var i = 0; i < mdl.Meshes.Length; ++i) {
                    materials[i] = mdl.Meshes[i].Material.Get(imcVar);

                    for (var j = 0; j < mdl.Meshes[i].Parts.Length; ++j) {
                        // TODO: Confirm this to actually be true.
                        var result = ((imcVar.PartVisibilityMask & mdl.Meshes[i].Parts[j].Header.VisibilityMask) == mdl.Meshes[i].Parts[j].Header.VisibilityMask);
                        Console.WriteLine("Mesh[{0}].Part[{1}].Visible = {2}", i, j, result);
                    }
                }


                System.Diagnostics.Debugger.Break();
            }

            {
                var p = packs.GetPack(new SaintCoinach.IO.PackIdentifier("chara", "ffxiv", 0));
                foreach (var mdlFile in p.OfType<SaintCoinach.Graphics.ModelFile>()) {
                    SaintCoinach.Graphics.ModelDefinition def = null;
                    try {
                        def = new SaintCoinach.Graphics.ModelDefinition(mdlFile);
                    } catch(NotSupportedException nse) {
                        // For all in chara/* if exception is in MaterialDefinition..ctor then it's some weird material that can't be located
                        if (nse.TargetSite.Name == ".ctor" && nse.TargetSite.DeclaringType != typeof(SaintCoinach.Graphics.MaterialDefinition))
                            continue;
                        throw;
                    }
                    foreach (var q in def.AvailableQualities)
                        def.GetModel(q);
                }
            }
        }
    }
}
