using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tharga.Console.Commands.Base;

#pragma warning disable CS1998

namespace SaintCoinach.Cmd.Commands {
    public class ExdCommand : AsyncActionCommandBase {
        private ARealmReversed _Realm;

        public ExdCommand(ARealmReversed realm)
            : base("exd", "Export all data (default), or only specific data files, seperated by spaces.") {
            _Realm = realm;
        }

        public override async Task InvokeAsync(string[] paramList) {
            const string CsvFileFormat = "exd/{0}.csv";

            IEnumerable<string> filesToExport;

            if (paramList.Length == 0)
                filesToExport = _Realm.GameData.AvailableSheets;
            else
                filesToExport = paramList.Select(_ => _Realm.GameData.FixName(_));

            var successCount = 0;
            var failCount = 0;
            foreach (var name in filesToExport) {
                var target = new FileInfo(Path.Combine(_Realm.GameVersion, string.Format(CsvFileFormat, name)));
                try {
                    var sheet = _Realm.GameData.GetSheet(name);

                    if (!target.Directory.Exists)
                        target.Directory.Create();

                    ExdHelper.SaveAsCsv(sheet, Ex.Language.None, target.FullName, false);

                    ++successCount;
                } catch (Exception e) {
                    OutputError($"Export of {name} failed: {e.Message}");
                    try { if (target.Exists) { target.Delete(); } } catch { }
                    ++failCount;
                }
            }
            OutputInformation($"{successCount} files exported, {failCount} failed");
        }
    }
}
