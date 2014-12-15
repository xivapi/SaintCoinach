using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

namespace SaintCoinach.Cmd.Commands {
    public class ExdCommand : ActionCommandBase {
        private ARealmReversed _Realm;

        public ExdCommand(ARealmReversed realm)
            : base("exd", "Export all data (default), or only specific data files, seperated by spaces.") {
            _Realm = realm;
        }

        public override async Task<bool> InvokeAsync(string paramList) {
            const string CsvFileFormat = "exd/{0}.csv";

            IEnumerable<string> filesToExport;

            if (string.IsNullOrWhiteSpace(paramList))
                filesToExport = _Realm.GameData.AvailableSheets;
            else
                filesToExport = paramList.Split(' ').Select(_ => _Realm.GameData.FixName(_));

            var successCount = 0;
            var failCount = 0;
            foreach (var name in filesToExport) {
                try {
                    var sheet = _Realm.GameData.GetSheet(name);

                    var target = new FileInfo(Path.Combine(_Realm.GameVersion, string.Format(CsvFileFormat, name)));
                    if (!target.Directory.Exists)
                        target.Directory.Create();

                    SaveAsCsv(sheet, target.FullName);

                    ++successCount;
                } catch (Exception e) {
                    OutputError("Export of {0} failed: {1}", name, e.Message);
                    ++failCount;
                }
            }
            OutputInformation("{0} files exported, {1} failed", successCount, failCount);

            return true;
        }

        public static void SaveAsCsv(Ex.Relational.IRelationalSheet sheet, string path) {
            using (var s = new StreamWriter(path, false, Encoding.UTF8)) {
                var indexLine = new StringBuilder("key");
                var nameLine = new StringBuilder("#");
                var typeLine = new StringBuilder("int32");

                var colIndices = new List<int>();
                foreach (var col in sheet.Header.Columns) {
                    indexLine.AppendFormat(",{0}", col.Index);
                    nameLine.AppendFormat(",{0}", col.Name);
                    typeLine.AppendFormat(",{0}", col.ValueType);

                    colIndices.Add(col.Index);
                }

                s.WriteLine(indexLine);
                s.WriteLine(nameLine);
                s.WriteLine(typeLine);

                foreach (var row in sheet.GetAllRows().OrderBy(_ => _.Key)) {
                    s.Write(row.Key);
                    foreach (var col in colIndices) {
                        var v = row[col];

                        if (v == null)
                            s.Write(",");
                        else if (IsUnescaped(v))
                            s.Write(",{0}", v);
                        else
                            s.Write(",\"{0}\"", v.ToString().Replace("\"", "\"\""));
                    }
                    s.WriteLine();
                }
            }
        }
        private static bool IsUnescaped(object self) {
            return (self is Boolean
                || self is Byte
                || self is SByte
                || self is Int16
                || self is Int32
                || self is Int64
                || self is UInt16
                || self is UInt32
                || self is UInt64
                || self is Single
                || self is Double);
        }
    }
}
