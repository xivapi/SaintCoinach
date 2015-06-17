using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

using SaintCoinach;
using SaintCoinach.Ex;
using SaintCoinach.Xiv;

namespace SaintCoinach.Cmd.Commands {
    public class AllExdCommand : ActionCommandBase {
        private ARealmReversed _Realm;

        public AllExdCommand(ARealmReversed realm)
            : base("allexd", "Export all data (default), or only specific data files, seperated by spaces; including all languages.") {
            _Realm = realm;
        }

        public override async Task<bool> InvokeAsync(string paramList) {
            const string CsvFileFormat = "exd-all/{0}{1}.csv";

            IEnumerable<string> filesToExport;

            if (string.IsNullOrWhiteSpace(paramList))
                filesToExport = _Realm.GameData.AvailableSheets;
            else
                filesToExport = paramList.Split(' ').Select(_ => _Realm.GameData.FixName(_));

            var successCount = 0;
            var failCount = 0;
            foreach (var name in filesToExport) {
                var sheet = _Realm.GameData.GetSheet(name);
                foreach(var lang in sheet.Header.AvailableLanguages) {
                    var code = lang.GetCode();
                    if (code.Length > 0)
                        code = "." + code;
                    var target = new FileInfo(Path.Combine(_Realm.GameVersion, string.Format(CsvFileFormat, name, code)));
                    try {

                        if (!target.Directory.Exists)
                            target.Directory.Create();

                        SaveAsCsv(sheet, lang, target.FullName);

                        ++successCount;
                    } catch (Exception e) {
                        OutputError("Export of {0} failed: {1}", name, e.Message);
                        try { if (target.Exists) { target.Delete(); } } catch { }
                        ++failCount;
                    }
                }
                
            }
            OutputInformation("{0} files exported, {1} failed", successCount, failCount);

            return true;
        }

        public static void SaveAsCsv(Ex.Relational.IRelationalSheet sheet, Language language, string path) {
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

                foreach (var row in sheet.Cast<Ex.IRow>().OrderBy(_ => _.Key)) {
                    var useRow = row;

                    if (useRow is IXivRow)
                        useRow = ((IXivRow)row).SourceRow;
                    var multiRow = useRow as IMultiRow;

                    s.Write(useRow.Key);
                    foreach (var col in colIndices) {
                        object v;

                        if (language == Language.None || multiRow == null)
                            v = useRow[col];
                        else
                            v = multiRow[col, language];

                        if (v == null)
                            s.Write(",");
                        else if (IsUnescaped(v))
                            s.Write(",{0}", v);
                        else
                            s.Write(",\"{0}\"", v.ToString().Replace("\"", "\"\""));
                    }
                    s.WriteLine();

                    s.Flush();
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
