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
    public class RawCommand : ActionCommandBase {
        private ARealmReversed _Realm;

        public RawCommand(ARealmReversed realm)
            : base("raw", "Save raw contents of a file.") {
            _Realm = realm;
        }

        public override async Task<bool> InvokeAsync(string paramList) {
            if (paramList == null)
                return false;
            try {
                if (_Realm.Packs.TryGetFile(paramList.Trim(), out var file)) {
                    var target = new FileInfo(Path.Combine(_Realm.GameVersion, file.Path));
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
