using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Cmd {
    using Ex;
    using Xiv;

    static class ExdHelper {
        public static void WriteRows(StreamWriter s, ISheet sheet, Language language, IEnumerable<int> colIndices, bool writeRaw) {
            if (sheet.Header.Variant == 1)
                WriteRowsCore(s, sheet.Cast<Ex.IRow>(), language, colIndices, writeRaw, WriteRowKey);
            else {
                var rows = sheet.Cast<XivRow>().Select(_ => (Ex.Variant2.DataRow)_.SourceRow);
                foreach (var parentRow in rows.OrderBy(_ => _.Key))
                    WriteRowsCore(s, parentRow.SubRows, language, colIndices, writeRaw, WriteSubRowKey);
            }
        }

        static void WriteRowsCore(StreamWriter s, IEnumerable<Ex.IRow> rows, Language language, IEnumerable<int> colIndices, bool writeRaw, Action<StreamWriter, Ex.IRow> writeKey) {
            foreach (var row in rows.OrderBy(_ => _.Key)) {
                var useRow = row;

                if (useRow is IXivRow)
                    useRow = ((IXivRow)row).SourceRow;
                var multiRow = useRow as IMultiRow;

                writeKey(s, useRow);
                foreach (var col in colIndices) {
                    object v;

                    if (language == Language.None || multiRow == null)
                        v = writeRaw ? useRow.GetRaw(col) : useRow[col];
                    else
                        v = writeRaw ? multiRow.GetRaw(col, language) : multiRow[col, language];

                    if (v == null)
                        s.Write(",");
                    else if (v is IDictionary<int, object>)
                        WriteDict(s, v as IDictionary<int, object>);
                    else if (IsUnescaped(v))
                        s.Write(",{0}", v);
                    else
                        s.Write(",\"{0}\"", v.ToString().Replace("\"", "\"\""));
                }
                s.WriteLine();

                s.Flush();
            }
        }

        static void WriteDict(StreamWriter s, IDictionary<int, object> v) {
            s.Write(",\"");
            var isFirst = true;
            foreach (var kvp in v) {
                if (isFirst)
                    isFirst = false;
                else
                    s.Write(",");
                s.Write("[{0},", kvp.Key);
                if (kvp.Value != null)
                    s.Write(kvp.Value.ToString().Replace("\"", "\"\""));
                s.Write("]");
            }
            s.Write("\"");
        }

        static void WriteRowKey(StreamWriter s, Ex.IRow row) {
            s.Write(row.Key);
        }

        static void WriteSubRowKey(StreamWriter s, Ex.IRow row) {
            var subRow = (Ex.Variant2.SubRow)row;
            s.Write(subRow.FullKey);
        }

        static bool IsUnescaped(object self) {
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
