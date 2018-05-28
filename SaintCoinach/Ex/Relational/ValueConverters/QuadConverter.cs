using Newtonsoft.Json.Linq;
using SaintCoinach.Ex.Relational.Definition;
using SaintCoinach.Xiv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.ValueConverters {
    public class QuadConverter : IValueConverter {

        #region IValueConverter Members

        public string TargetTypeName => "Quad";

        public Type TargetType => typeof(Quad);

        public object Convert(IDataRow row, object rawValue) {
            return new Quad((long)rawValue);
        }

        #endregion

        #region Serialization

        public JObject ToJson() {
            return new JObject() {
                ["type"] = "quad"
            };
        }

        public static QuadConverter FromJson(JToken obj) {
            return new QuadConverter();
        }

        public void ResolveReferences(SheetDefinition sheetDef) { }

        #endregion
    }
}
