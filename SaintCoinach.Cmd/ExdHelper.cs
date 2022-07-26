using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SaintCoinach.Cmd {
    using Ex;
    using Xiv;

    static class ExdHelper {
        static CultureInfo _culture = new CultureInfo("en-US", false);

        public static void SaveAsCsv(Ex.Relational.IRelationalSheet sheet, Language language, string path, bool writeRaw) {
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

                ExdHelper.WriteRows(s, sheet, language, colIndices, writeRaw);
            }
        }

        public static void SaveAsJson(Ex.Relational.IRelationalSheet sheet, Language language, string path, bool writeRaw) {
            using (var s = new StreamWriter(path, false, Encoding.UTF8)) {
                var data = new Dictionary<string, object>();

                var cols = new Dictionary<int, string>();
                var items = new Dictionary<string, object>();

                foreach (var col in sheet.Header.Columns) {
                    string key = "";
                    if (col.Name != null)
                        key = col.Name.Replace("{", ".").Replace("[", ".").Replace("}", "").Replace("]", "");
                    cols.Add(col.Index, key);
                }

                var rows = new List<IEnumerable<Ex.IRow>>();
                if (sheet.Header.Variant == 1) {
                    rows.Add(sheet.Cast<Ex.IRow>());
                } else {
                    var parentRows = sheet.Cast<XivRow>().Select(_ => (Ex.Variant2.DataRow)_.SourceRow);
                    foreach (var parentRow in parentRows.OrderBy(_ => _.Key))
                        rows.Add(parentRow.SubRows);
                }

                foreach (var sub in rows) {
                    foreach (var row in sub.OrderBy(_ => _.Key)) {
                        var useRow = row;
                        if (useRow is IXivRow)
                            useRow = ((IXivRow)row).SourceRow;
                        var multiRow = useRow as IMultiRow;

                        var insert = new Dictionary<string, object>();

                        string itemKey = row.Key.ToString();
                        if (sheet.Header.Variant != 1)
                            itemKey = ((Ex.Variant2.SubRow)row).FullKey;
                        SetJsonKey(insert, "Key", itemKey);

                        foreach (KeyValuePair<int, string> item in cols) {
                            int index = item.Key;
                            string key = item.Value;
                            if (key == "")
                                key = "col_" + index;

                            object v;

                            if (language == Language.None || multiRow == null)
                                v = writeRaw ? useRow.GetRaw(index) : useRow[index];
                            else
                                v = writeRaw ? multiRow.GetRaw(index, language) : multiRow[index, language];

                            if (v == null)
                                continue;
                            SetJsonKey(insert, key, v.ToString());
                        }

                        items.Add(itemKey, insert);
                    }
                }

                s.WriteLine(JsonConvert.SerializeObject(items, cols.Count<=100? Formatting.Indented : Formatting.None));
            }
        }

        public static void SetJsonKey(Dictionary<string, object> dict, string path, string val) {
            var keys = path.Split('.');
            var toSet = dict;
            for (int i = 0; i<keys.Length; i++) {
                var key = keys[i];
                if (i == keys.Length - 1) {
                    toSet[key] = val;
                } else {
                    if (!toSet.ContainsKey(key))
                        toSet.Add(key, new Dictionary<string, object>());

                    if (toSet[key] is String) {
                        var mod = new Dictionary<string, object>();
                        mod.Add("Value", toSet[key]);
                        toSet[key] = mod;
                    }
                    toSet = (Dictionary<string, object>)toSet[key];
                }
            }
        }

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

                    s.Write(",");
                    if (v == null)
                        continue;
                    else if (IsUnescaped(v))
                        s.Write(string.Format(_culture, "{0}", v));
                    else
                        s.Write("\"{0}\"", v.ToString().Replace("\"", "\"\""));
                }
                s.WriteLine();

                s.Flush();
            }
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
