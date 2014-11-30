using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

namespace SaintCoinach.Cmd {
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

        public GraphicsCommand(IO.PackCollection pack, Xiv.XivCollection data)
            : base("3d") {
                SubCommands.Add(new ModelCommand(pack, data));
                SubCommands.Add(new MapCommand(pack, data));
                SubCommands.Add(new MonsterCommand(pack, data));
        }

        static IEnumerable<string> ApplyMaterial(Graphics.Model model, string[] parts, int offset) {

            if (parts.Length > offset) {
                int v;
                if (int.TryParse(parts[offset], out v)) {
                    int? stain = null;
                    if (parts.Length > offset + 1) {
                        int temp;
                        if (int.TryParse(parts[offset + 1], out temp))
                            stain = temp;
                    }

                    var stainAll = true;
                    var versionAll = true;

                    foreach (var mesh in model.Meshes) {
                        if (mesh.AvailableMaterialVersions.Contains(v))
                            mesh.MaterialVersion = v;
                        else
                            versionAll = false;
                        if (stain.HasValue && mesh.CanStain && mesh.AvailableStains.Contains(stain.Value))
                            mesh.MaterialStain = stain.Value;
                        else if (stain.HasValue)
                            stainAll = false;
                    }

                    if (!versionAll)
                        yield return ("Version not present on all materials, using default on some.");
                    if (!stainAll)
                        yield return ("Stain not present on all materials, using default on some.");
                } else
                    yield return ("Version input could not be parsed.");
            }
        }

        static void RunViewer(Graphics.IComponent component, string title) {
            var thread = new Thread(RunViewerAsync);
            thread.IsBackground = true;
            thread.Name = "3D";
            thread.Start(Tuple.Create(component, title));
        }

        static void RunViewerAsync(object state) {
            var t = (Tuple<Graphics.IComponent, string>)state;

            var e = new Graphics.ViewerEngine(t.Item2);

            e.Add(t.Item1);

            e.Run();
        }
    }
}
