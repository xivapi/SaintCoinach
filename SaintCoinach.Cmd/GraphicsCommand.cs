using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

namespace SaintCoinach.Cmd {/*
    public class GraphicsCommand : ContainerCommandBase {
        class ModelCommand : ActionCommandBase {
            private IO.PackCollection _Pack;
            private Xiv.XivCollection _Data;

            public ModelCommand(IO.PackCollection pack, Xiv.XivCollection data)
                : base("model", "Display a model.") {
                _Pack = pack;
                _Data = data;
            }

            public async override Task<bool> InvokeAsync(string paramList) {
                if (string.IsNullOrWhiteSpace(paramList)) {
                    paramList = @"chara/monster/m0099/obj/body/b0001/model/m0099b0001.mdl";
                    //paramList = @"chara/equipment/e0124/model/c1101e0124_top.mdl 1 83";
                    //paramList = @"chara/equipment/e0075/model/c1101e0075_top.mdl 1 112";
                }

                var splitParam = paramList.Split(' ');

                IO.File file;
                if (_Pack.TryGetFile(splitParam[0], out file)) {
                    var mdlFile = file as Graphics.Assets.ModelFile;
                    if (mdlFile != null) {
                        var mdl = mdlFile.GetModel();
                        var subMdl = mdl.GetSubModel(0);

                        var component = new Graphics.Model(subMdl);
                        foreach (var msg in ApplyMaterial(component, splitParam, 1))
                            OutputWarning(msg);

                        RunViewer(component, file.Name);
                    } else
                        OutputError("File is not a model file.");
                } else
                    OutputError("File could not be found.");

                return true;
            }
        }
        class MapCommand : ActionCommandBase {
            private IO.PackCollection _Pack;
            private Xiv.XivCollection _Data;
            public MapCommand(IO.PackCollection pack, Xiv.XivCollection data)
                : base("map", "Display a map.") {
                _Pack = pack;
                _Data = data;
            }

            public async override Task<bool> InvokeAsync(string paramList) {
                if (string.IsNullOrWhiteSpace(paramList))
                    paramList = "bg/ffxiv/sea_s1/hou/s1h1";
                var component = new Graphics.Map(_Pack, paramList);
                RunViewer(component, paramList);

                return true;
            }
        }
        class MonsterCommand : ActionCommandBase {
            private IO.PackCollection _Pack;
            private Xiv.XivCollection _Data;
            public MonsterCommand(IO.PackCollection pack, Xiv.XivCollection data)
                : base("monster", "Display a monster.") {
                _Pack = pack;
                _Data = data;
            }

            public async override Task<bool> InvokeAsync(string paramList) {
                var splitParam = paramList.Split(' ');

                int m, b;
                if (splitParam.Length >= 1 && int.TryParse(splitParam[0], out m)) {
                    if (splitParam.Length < 2 || !int.TryParse(splitParam[1], out b))
                        b = 1;

                    var path = string.Format("chara/monster/m{0:D4}/obj/body/b{1:D4}/model/m{0:D4}b{1:D4}.mdl", m, b);

                    IO.File file;
                    if (_Pack.TryGetFile(path, out file)) {
                        var mdlFile = file as Graphics.Assets.ModelFile;
                        if (mdlFile != null) {
                            var mdl = mdlFile.GetModel();
                            var subMdl = mdl.GetSubModel(0);

                            var component = new Graphics.Model(subMdl);
                            foreach (var msg in ApplyMaterial(component, splitParam, 2))
                                OutputWarning(msg);

                            RunViewer(component, file.Name);
                        } else
                            OutputError("File is not a model file.");
                    } else
                        OutputError("File could not be found.");
                } else
                    OutputError("Unable to parse input.");

                return true;
            }
        }
        class WeaponCommand : ActionCommandBase {
            private IO.PackCollection _Pack;
            private Xiv.XivCollection _Data;
            public WeaponCommand(IO.PackCollection pack, Xiv.XivCollection data)
                : base("weapon", "Display a weapon.") {
                _Pack = pack;
                _Data = data;
            }

            public async override Task<bool> InvokeAsync(string paramList) {
                var wI = _Data.GetSheet<Xiv.Item>().FirstOrDefault(_ => string.Equals(_.Name, paramList.Trim(), StringComparison.OrdinalIgnoreCase)) as Xiv.Items.Equipment;
                if (wI == null) {
                    OutputError("Weapon not found.");
                    return false;
                }
                var mdlKey = Convert.ToInt64(wI["Model{Main}"]);
                var w = mdlKey & 0xFFFF;
                var b = (mdlKey >> 16) & 0xFFFF;
                var v = (mdlKey >> 32) & 0xFFFF;

                var path = string.Format("chara/weapon/w{0:D4}/obj/body/b{1:D4}/model/w{0:D4}b{1:D4}.mdl", w, b);

                IO.File file;
                if (_Pack.TryGetFile(path, out file)) {
                    var mdlFile = file as Graphics.Assets.ModelFile;
                    if (mdlFile != null) {
                        var mdl = mdlFile.GetModel();
                        var subMdl = mdl.GetSubModel(0);

                        var component = new Graphics.Model(subMdl);
                        foreach (var msg in ApplyMaterial(component, new[] { v.ToString() }, 0))
                            OutputWarning(msg);

                        RunViewer(component, file.Name);
                    } else
                        OutputError("File is not a model file.");
                } else
                    OutputError("File could not be found.");

                return true;
            }
        }

        public GraphicsCommand(IO.PackCollection pack, Xiv.XivCollection data)
            : base("3d") {
                SubCommands.Add(new ModelCommand(pack, data));
                SubCommands.Add(new MapCommand(pack, data));
                SubCommands.Add(new MonsterCommand(pack, data));
                SubCommands.Add(new WeaponCommand(pack, data));
        }

    }*/
}
