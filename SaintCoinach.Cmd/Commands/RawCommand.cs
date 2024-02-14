using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tharga.Console.Commands.Base;

#pragma warning disable CS1998

namespace SaintCoinach.Cmd.Commands {
    public class RawCommand : AsyncActionCommandBase {
        private ARealmReversed _Realm;

        public RawCommand(ARealmReversed realm)
            : base("raw", "Save raw contents of a file.") {
            _Realm = realm;
        }

        public override async Task InvokeAsync(string[] paramList) {
            if (paramList == null)
                return;
            
            try {
                if (_Realm.Packs.TryGetFile(paramList[0].Trim(), out var file)) {
                    var target = new FileInfo(Path.Combine(_Realm.GameVersion, file.Path));
                    if (!target.Directory.Exists)
                        target.Directory.Create();

                    var data = file.GetData();
                    File.WriteAllBytes(target.FullName, data);
                } else
                    OutputError("File not found.");
            } catch (Exception e) {
                OutputError(e, true);
            }
        }
    }
}
