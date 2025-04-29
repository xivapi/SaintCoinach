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
                var path = paramList[0].Trim();
                if (_Realm.Packs.TryGetFile(path, out var file)) {
                    Imaging.ImageFile imgFile = null;
                    if (path.EndsWith(".atex")) {
                        imgFile = new Imaging.ImageFile((IO.FileDefault) file);
                    } else if (file is Imaging.ImageFile) {
                        imgFile = (Imaging.ImageFile) file;
                    }

                    if (imgFile is null) {
                        OutputError($"File is not an image (actual: {file.CommonHeader.FileType}).");
                        return;
                    }

                    var img = imgFile.GetImage();

                    var target = new FileInfo(Path.Combine(_Realm.GameVersion, file.Path));
                    if (!target.Directory.Exists)
                        target.Directory.Create();
                    var pngPath = target.FullName.Substring(0, target.FullName.Length - target.Extension.Length) + ".png";
                    img.Save(pngPath);
                        
                } else
                    OutputError("File not found.");
            } catch (Exception e) {
                OutputError(e.Message);
            }
        }
    }
}
