using Newtonsoft.Json.Linq;
using SaintCoinach.Ex.Relational.Definition;
using System;

namespace SaintCoinach.Ex.Relational.ValueConverters {
    public class SheetLinkConverter : IValueConverter {
        #region Properties

        public string TargetSheet { get; set; }

        #endregion

        #region IValueConverter Members

        public string TargetTypeName { get { return TargetSheet; } }
        public Type TargetType { get { return typeof(IRelationalRow); } }

        public object Convert(IDataRow row, object rawValue) {
            var coll = row.Sheet.Collection;
            if (!coll.SheetExists(TargetSheet))
                return null;

            var sheet = coll.GetSheet(TargetSheet);

            var key = System.Convert.ToInt32(rawValue);
            return !sheet.ContainsRow(key) ? null : sheet[key];
        }

        #endregion

        #region Serialization

        public JObject ToJson() {
            return new JObject() {
                ["type"] = "link",
                ["target"] = TargetSheet
            };
        }

        public static SheetLinkConverter FromJson(JToken obj) {
            return new SheetLinkConverter() {
                TargetSheet = (string)obj["target"]
            };
        }

        public void ResolveReferences(SheetDefinition sheetDef) { }

        #endregion
    }
}
