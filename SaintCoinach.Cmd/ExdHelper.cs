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
        public static void WriteRows(StreamWriter s, ISheet sheet, Language language, IEnumerable<int> colIndices) {
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
