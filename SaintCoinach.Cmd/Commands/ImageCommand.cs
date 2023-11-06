using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tharga.Console.Commands.Base;

#pragma warning disable CS1998

namespace SaintCoinach.Cmd.Commands {
    public class ImageCommand : AsyncActionCommandBase {
        private ARealmReversed _Realm;

        public ImageCommand(ARealmReversed realm)
            : base("image", "Export an image file.") {
            _Realm = realm;
        }

        public override async Task InvokeAsync(string[] paramList) {
            try {
                if (_Realm.Packs.TryGetFile(paramList[0].Trim(), out var file)) {
                    if (file is Imaging.ImageFile imgFile) {
                        var img = imgFile.GetImage();

                        var target = new FileInfo(Path.Combine(_Realm.GameVersion, file.Path));
                        if (!target.Directory.Exists)
                            target.Directory.Create();
                        var pngPath = target.FullName.Substring(0, target.FullName.Length - target.Extension.Length) + ".png";
                        img.Save(pngPath);
                    } else
                        OutputError($"File is not an image (actual: {file.CommonHeader.FileType}).");
                } else
                    OutputError("File not found.");
            } catch (Exception e) {
                OutputError(e.Message);
            }
        }
    }
}
