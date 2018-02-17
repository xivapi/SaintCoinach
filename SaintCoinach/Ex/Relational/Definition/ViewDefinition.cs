using Newtonsoft.Json.Linq;
using SaintCoinach.Ex.Relational;
using SaintCoinach.Ex.Relational.ValueConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Definition {
    public class ViewDefinition {
        public string SheetName { get; internal set; }
        public ICollection<ViewColumnDefinition> ColumnDefinitions { get; internal set; }

        public static ViewDefinition FromJson(JToken obj) {
            return new ViewDefinition() {
                SheetName = (string)obj["sheet"],
                ColumnDefinitions = obj["columns"].Cast<JObject>().Select(ViewColumnDefinition.FromJson).ToList()
            };
        }
    }

    public class ViewColumnDefinition {
        public string ColumnName { get; internal set; }
        public IValueConverter Converter { get; internal set; }

        public static ViewColumnDefinition FromJson(JObject obj) {
            var converterObj = (JObject)obj["converter"];
            IValueConverter converter = null;
            if (converterObj != null) {
                var type = (string)converterObj["type"];
                if (type == "quad")
                    converter = QuadConverter.FromJson(converterObj);
                else
                    throw new ArgumentException("Invalid converter type.", "obj");
            }

            return new ViewColumnDefinition() {
                ColumnName = (string)obj["name"],
                Converter = converter
            };
        }
    }
}
