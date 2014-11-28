using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

namespace SaintCoinach.Cmd {
    public class TempCommand : ActionCommandBase {
        private IO.PackCollection _Pack;
        private Xiv.XivCollection _Data;

        public TempCommand(IO.PackCollection pack, Xiv.XivCollection data)
            : base("temp", "Haw haw") {
            _Pack = pack;
            _Data = data;
        }

        public async override Task<bool> InvokeAsync(string paramList) {
            ModelParameterGroups(paramList);

            return true;
        }
        private void ModelParameterGroups(string paramList) {
            if (string.IsNullOrWhiteSpace(paramList)) {
                return;
            }
            var models = File.ReadAllLines(@"A:\Users\Kalce\Documents\XIV\SkinModels.txt");

            var rnd = new Random();

            var groups = new Dictionary<uint[], List<string>>();
            foreach (var model in models) {
                try {
                    var mdlF = (Graphics.Assets.ModelFile)_Pack.GetFile(model);
                    var mdl = mdlF.GetModel();
                    var subMdl = mdl.GetSubModel(0);
                    foreach (var mesh in subMdl.Meshes) {
                        foreach (var v in mesh.Material.AvailableVersions) {
                            var mtrlV = mesh.Material.GetVersion(v);
                            if (mtrlV.Shader == paramList)
                                AnalyzeParameterGroups(mtrlV, groups);

                            if (mtrlV.CanStain) {
                                foreach (var s in mtrlV.AvailableStains.OrderBy(_ => rnd.NextDouble()).Take(2)) {
                                    mtrlV.CurrentStain = s;
                                    AnalyzeParameterGroups(mtrlV, groups);
                                }
                            }
                        }
                    }
                } catch (Exception e) {
                    OutputError("Error in {0}: {1}", model, e.Message);
                }
            }
            using (var log = new StreamWriter(string.Format("MaterialParam - {0}.log", paramList))) {
                foreach (var group in groups) {
                    log.WriteLine("{");
                    foreach (var id in group.Key)
                        log.WriteLine("  0x{0:X8}", id);
                    log.WriteLine("};");
                }
                log.WriteLine();

                foreach (var group in groups) {

                    log.WriteLine("{");
                    foreach (var id in group.Key)
                        log.WriteLine("  0x{0:X8}", id);
                    log.WriteLine("} = [");
                    foreach (var f in group.Value)
                        log.WriteLine("  {0}", f);
                    log.WriteLine("]");
                }
            }
        }
        private void AnalyzeParameterGroups(Graphics.Assets.MaterialVersion v, Dictionary<uint[], List<string>> output) {
            var idList = new List<uint>();
            foreach (var pm in v.ParameterMappings)
                idList.Add(pm.Id);
            var ids = idList.OrderBy(_ => _).ToArray();

            var res = output.Where(_ => Equals(ids, _.Key));
            List<string> fileList = new List<string>();
            if (res.Any())
                fileList = output[res.First().Key];
            else
                output.Add(ids, fileList = new List<string>());

            fileList.Add(v.CurrentFile.Path);
        }
        private bool Equals<T>(T[] left, T[] right) where T : IEquatable<T> {
            if (left.Length != right.Length)
                return false;
            for (var i = 0; i < left.Length; ++i) {
                if (!left[i].Equals(right[i]))
                    return false;
            }
            return true;
        }
        private void ShPkParameters(string paramList) {
            IO.File file;
            if (_Pack.TryGetFile(paramList, out file)) {
                var shpk = new Graphics.ShPk.ShPkFile(file);

                var logFile = new FileInfo(file.Path + ".log");
                if (!logFile.Directory.Exists)
                    logFile.Directory.Create();
                using (var log = new StreamWriter(logFile.FullName)) {
                    foreach (var p in shpk.Parameters)
                        log.WriteLine("{{ 0x{0:X8}, \"{1}\", }},", p.Id, p.Name);
                }
            }
        }
    }
}
