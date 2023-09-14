using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tharga.Console.Commands.Base;

#pragma warning disable CS1998

namespace SaintCoinach.Cmd.Commands {
    public class RawExdCommand : AsyncActionCommandBase {
        private ARealmReversed _Realm;

        public RawExdCommand(ARealmReversed realm)
            : base("rawexd", "Export all data (default), or only specific data files, seperated by spaces. No post-processing is applied to values.") {
            _Realm = realm;
        }

        public override async Task InvokeAsync(string[] paramList) {
            const string CsvFileFormat = "rawexd/{0}.csv";

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

                    ExdHelper.SaveAsCsv(sheet, Ex.Language.None, target.FullName, true);

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
