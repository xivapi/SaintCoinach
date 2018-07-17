using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

#pragma warning disable CS1998

namespace SaintCoinach.Cmd.Commands {
    public class UiCommand : ActionCommandBase {
        const string UiImagePathFormat = "ui/icon/{0:D3}000{1}/{2:D6}.tex";
        static readonly string[] UiVersions = new string[] {
            "",
            "/en",
            "/ja",
            "/fr",
            "/de",
            "/hq"
        };

        private ARealmReversed _Realm;

        public UiCommand(ARealmReversed realm)
            : base("ui", "Export all, a single, or a range of UI icons.") {
            _Realm = realm;
        }

        public override async Task<bool> InvokeAsync(string paramList) {
            var min = 0;
            var max = 999999;

            if (!string.IsNullOrWhiteSpace(paramList)) {
                var splitParam = paramList.Split(' ');

                if (splitParam.Length == 1) {
                    if (int.TryParse(splitParam[0], out var parsed))
                        min = max = parsed;
                    else {
                        OutputError("Failed to parse parameters.");
                        return false;
                    }
                } else if (splitParam.Length == 2) {
                    if (!int.TryParse(splitParam[0], out min) || !int.TryParse(splitParam[1], out max)) {
                        OutputError("Failed to parse parameters.");
                        return false;
                    }

                    if (max < min) {
                        OutputError("Invalid parameters.");
                        return false;
                    }
                } else {
                    OutputError("Failed to parse parameters.");
                    return false;
                }
            }

            var count = 0;
            for (int i = min; i <= max; ++i) {
                try {
                    count += Process(i);
                } catch (Exception e) {
                    OutputError("{0:D6}: {1}", i, e.Message);
                }
            }
            OutputInformation("{0} images processed", count);

            return true;
        }

        private int Process(int i) {
            var count = 0;
            foreach (var v in UiVersions) {
                if (Process(i, v))
                    ++count;
            }
            return count;
        }
        private bool Process(int i, string version) {
            var filePath = string.Format(UiImagePathFormat, i / 1000, version, i);

            if (_Realm.Packs.TryGetFile(filePath, out var file)) {
                if (file is Imaging.ImageFile imgFile) {
                    var img = imgFile.GetImage();

                    var target = new FileInfo(Path.Combine(_Realm.GameVersion, file.Path));
                    if (!target.Directory.Exists)
                        target.Directory.Create();
                    var pngPath = target.FullName.Substring(0, target.FullName.Length - target.Extension.Length) + ".png";
                    img.Save(pngPath);

                    return true;
                } else {
                    OutputError("{0} is not an image.", filePath);
                }
            }
            return false;
        }
    }
}
