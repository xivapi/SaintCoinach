using Newtonsoft.Json.Linq;
using SaintCoinach.Ex.Relational.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.ValueConverters {
    public class MultiReferenceConverter : IValueConverter {
        #region Properties

        public string[] Targets { get; set; }

        #endregion

        #region IValueConverter Members

        public string TargetTypeName { get { return "Row"; } }
        public Type TargetType { get { return typeof(IRelationalRow); } }

        public object Convert(IDataRow row, object rawValue) {
            if (Targets == null)
                return null;

            var key = System.Convert.ToInt32(rawValue);

            foreach (var target in Targets) {
                var sheet = row.Sheet.Collection.GetSheet(target);
                if (!sheet.Header.DataFileRanges.Any(r => r.Contains(key)))
                    continue;

                if (sheet.ContainsRow(key))
                    return sheet[key];
            }
            return null;
        }

        #endregion

        #region Serialization

        public JObject ToJson() {
            return new JObject() {
                ["type"] = "multiref",
                ["targets"] = new JArray(Targets)
            };
        }

        public static MultiReferenceConverter FromJson(JToken obj) {
            return new MultiReferenceConverter() {
                Targets = obj["targets"].Select(t => (string)t).ToArray()
            };
        }

        public void ResolveReferences(SheetDefinition sheetDef) { }

        #endregion
    }
}
