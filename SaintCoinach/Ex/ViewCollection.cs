using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SaintCoinach.Ex.Relational.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public class ViewCollection {
        private Dictionary<string, ViewDefinition> _ViewDefinitionsBySheetName;

        public static ViewCollection FromJson(string json) {
            var obj = JsonConvert.DeserializeObject<JObject>(json);
            return FromJson(obj);
        }

        public static ViewCollection FromJson(JObject obj) {
            return new ViewCollection() {
                _ViewDefinitionsBySheetName = obj["views"].Select(ViewDefinition.FromJson).ToDictionary(v => v.SheetName)
            };
        }
    }
}
