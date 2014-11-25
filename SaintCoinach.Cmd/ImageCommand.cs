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
    public class ImageCommand : ActionCommandBase {
        private IO.PackCollection _Pack;

        public ImageCommand(IO.PackCollection pack)
            : base("image", "Export an image file.") {
            _Pack = pack;
        }

        public override async Task<bool> InvokeAsync(string paramList) {
            try {
                IO.File file;
                if (_Pack.TryGetFile(paramList.Trim(), out file)) {
                    var imgFile = file as Imaging.ImageFile;
                    if (imgFile != null) {
                        var img = imgFile.GetImage();

                        var target = new FileInfo(file.Path);
                        if (!target.Directory.Exists)
                            target.Directory.Create();
                        var pngPath = target.FullName.Substring(0, target.FullName.Length - target.Extension.Length) + ".png";
                        img.Save(pngPath);
                    } else
                        OutputError("File is not an image.");
                } else
                    OutputError("File not found.");
            } catch (Exception e) {
                OutputError(e.Message);
            }

            return true;
        }
    }
}
