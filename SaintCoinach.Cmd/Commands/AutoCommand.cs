using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

using G = SaintCoinach.Graphics.Assets;

namespace SaintCoinach.Cmd.Commands {
    public class AutoCommand : ActionCommandBase {
        private ARealmReversed _Realm;

        public AutoCommand(ARealmReversed realm)
            : base("auto", "Save file and things referenced by it.") {
            _Realm = realm;
        }

        public override async Task<bool> InvokeAsync(string paramList) {
            if (paramList == null)
                return false;
            try {
                var l = new List<IO.IIndexFile>();
                IO.File file;
                if (_Realm.Packs.TryGetFile(paramList.Trim(), out file)) {
                    Save(file, l);
                } else
                    OutputError("File not found.");
            } catch (Exception e) {
                OutputError(e.Message);
            }

            return true;
        }

        void Save(IO.File file, List<IO.IIndexFile> saved) {
            if (saved.Contains(file.Index))
                return;
            saved.Add(file.Index);

            var target = new FileInfo(Path.Combine(_Realm.GameVersion, file.Path));
            if (!target.Directory.Exists)
                target.Directory.Create();

            if (file is G.ModelFile)
                SaveModel((G.ModelFile)file, target, saved);
            else {
                if (file is Imaging.ImageFile)
                    SaveImage((Imaging.ImageFile)file, target, saved);

                var data = file.GetData();
                File.WriteAllBytes(target.FullName, data);
            }
        }

        void SaveImage(Imaging.ImageFile file, FileInfo target, List<IO.IIndexFile> saved) {
            var img = file.GetImage();
            img.Save(target.FullName.Substring(0, target.FullName.Length - target.Extension.Length) + ".png", System.Drawing.Imaging.ImageFormat.Png);
        }
        void SaveModel(G.ModelFile file, FileInfo target, List<IO.IIndexFile> saved) {
            for (var i = 0; i < G.ModelFile.PartsCount; ++i) {
                var part = file.GetPart(i);
                File.WriteAllBytes(target.FullName + "." + i.ToString("D2"), part);
            }

            try {
                var mdl = file.GetModel();
                for (var i = 0; i < mdl.MaterialCount; ++i) {
                    try {
                        var mtrl = mdl.GetMaterial(i);
                        SaveMaterial(mtrl, saved);
                    } catch (Exception e) {
                        OutputError("Model '{0}', Material #{1}: {2}", file.Path, i, e.Message);
                    }
                }
            } catch (Exception e) {
                OutputError("Model '{0}' get: {1}", file.Path, e.Message);
            }
        }
        void SaveMaterial(G.Material mtrl, List<IO.IIndexFile> saved) {
            foreach (var v in mtrl.AvailableVersions) {
                try {
                    var mv = mtrl.GetVersion(v);

                    if (mv.CanStain) {
                        foreach (var s in mv.AvailableStains) {
                            mv.CurrentStain = s;
                            SaveMaterialVersion(mv, saved);
                        }
                    } else
                        SaveMaterialVersion(mv, saved);
                } catch(Exception e) {
                    OutputError("Material '{0}', V {1}: {2}", mtrl.VersionFormat, v, e.Message);
                }
            }
        }
        void SaveMaterialVersion(G.MaterialVersion mtrlV, List<IO.IIndexFile> saved) {
            Save(mtrlV.CurrentFile, saved);
            foreach (var tex in mtrlV.Textures)
                Save(tex, saved);
        }
    }
}
