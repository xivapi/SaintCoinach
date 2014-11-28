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
                    //paramList = @"bg/ffxiv/sea_s1/hou/s1h1/bgplate/0031.mdl";
                    paramList = @"chara/equipment/e9041/model/c0101e9041_top.mdl";
                    //paramList = @"chara/human/c0101/obj/face/f0006/model/c0101f0006_fac.mdl";
                    paramList = @"chara/human/c1201/obj/hair/h0001/model/c1201h0001_hir.mdl";
                }

                var splitParam = paramList.Split(' ');

                IO.File file;
                if (_Pack.TryGetFile(splitParam[0], out file)) {
                    var mdlFile = file as Graphics.Assets.ModelFile;
                    if (mdlFile != null) {
                        var mdl = mdlFile.GetModel();
                        var subMdl = mdl.GetSubModel(0);

                        var component = new Graphics.Model(subMdl);

                        if (splitParam.Length > 1) {
                            int v;
                            if (int.TryParse(splitParam[1], out v)) {
                                int? stain = null;
                                if (splitParam.Length > 2) {
                                    int temp;
                                    if (int.TryParse(splitParam[2], out temp))
                                        stain = temp;
                                }

                                var stainAll = true;
                                var versionAll = true;

                                foreach (var mesh in component.Meshes) {
                                    if (mesh.AvailableMaterialVersions.Contains(v))
                                        mesh.MaterialVersion = v;
                                    else
                                        versionAll = false;
                                    if (stain.HasValue && mesh.CanStain && mesh.AvailableStains.Contains(stain.Value))
                                        mesh.MaterialStain = stain.Value;
                                    else if(stain.HasValue)
                                        stainAll = false;
                                }

                                if (!versionAll)
                                    OutputWarning("Version not present on all materials, using default on some.");
                                if (!stainAll)
                                    OutputWarning("Stain not present on all materials, using default on some.");
                            } else
                                OutputError("Version input could not be parsed.");
                        }

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

        public GraphicsCommand(IO.PackCollection pack, Xiv.XivCollection data)
            : base("3d") {
                SubCommands.Add(new ModelCommand(pack, data));
                SubCommands.Add(new MapCommand(pack, data));
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
    /*
    public class GraphicsCommand : ActionCommandBase {
        private IO.PackCollection _Pack;
        private Xiv.XivCollection _Data;

        public GraphicsCommand(IO.PackCollection pack, Xiv.XivCollection data)
            : base("3d", "Just for testing.") {
            _Pack = pack;
            _Data = data;
        }

        public async override Task<bool> InvokeAsync(string paramList) {
            var thread = new Thread(RunEngineAsync);
            thread.IsBackground = true;
            thread.Name = "3D";

            var eng = new Graphics.ViewerEngine("Rawr");

            thread.Start(eng);

            return true;
        }

        private void RunEngineAsync(object state) {
            var e = (Graphics.ViewerEngine)state;
            var mdlF = (Graphics.Assets.ModelFile)_Pack.GetFile(@"chara/monster/m8005/obj/body/b0001/model/m8005b0001.mdl");
            var mdl = mdlF.GetModel();
            var subMdl = mdl.GetSubModel(0);
            var mesh = subMdl.Meshes.ElementAt(0);
            var vMesh = new Graphics.Parts.Mesh(mesh);

            e.Add(vMesh);
            
            e.Run();
        }
    }*/
}
