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
    public class RawCommand : ActionCommandBase {
        private IO.PackCollection _Pack;

        public RawCommand(IO.PackCollection pack)
            : base("raw", "Save raw contents of a file.") {
            _Pack = pack;
        }

        public override async Task<bool> InvokeAsync(string paramList) {
            try {
                IO.File file;
                if (_Pack.TryGetFile(paramList.Trim(), out file)) {
                    var target = new FileInfo(file.Path);
                    if (!target.Directory.Exists)
                        target.Directory.Create();

                    var data = file.GetData();
                    File.WriteAllBytes(target.FullName, data);
                } else
                    OutputError("File not found.");
            } catch (Exception e) {
                OutputError(e.Message);
            }

            return true;
        }
    }
}
