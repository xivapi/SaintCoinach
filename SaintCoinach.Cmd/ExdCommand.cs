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
    public class ExdCommand : ActionCommandBase {
        private Xiv.XivCollection _Data;

        public ExdCommand(Xiv.XivCollection data)
            : base("exd", "Export all data (default), or only specific data files, seperated by spaces.") {
            _Data = data;
        }

        public override async Task<bool> InvokeAsync(string paramList) {
            const string CsvFileFormat = "exd/{0}.csv";

            IEnumerable<string> filesToExport;

            if (string.IsNullOrWhiteSpace(paramList))
                filesToExport = _Data.AvailableSheets;
            else
                filesToExport = paramList.Split(' ').Select(_ => _Data.FixName(_));

            var successCount = 0;
            var failCount = 0;
            foreach (var name in filesToExport) {
                try {
                    var sheet = _Data.GetSheet(name);

                    var target = new FileInfo(string.Format(CsvFileFormat, name));
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
                var nameLine = new StringBuilder("");
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

                foreach (var row in sheet.GetAllRows()) {
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
