using Newtonsoft.Json.Linq;
using SaintCoinach.Ex.Relational.ValueConverters;
using System;

namespace SaintCoinach.Ex.Relational.Definition {
    public class SingleDataDefinition : IDataDefinition {
        #region Properties

        public string Name { get; set; }
        public IValueConverter Converter { get; set; }

        #endregion

        public int Length { get { return 1; } }

        public IDataDefinition Clone() {
            return new SingleDataDefinition {
                Name = Name,
                Converter = Converter
            };
        }

        #region IDataDefinition Members

        public object Convert(IDataRow row, object value, int index) {
            if (index != 0)
                throw new ArgumentOutOfRangeException("index");

            return Converter != null ? Converter.Convert(row, value) : value;
        }

        public string GetName(int index) {
            if (index != 0)
                throw new ArgumentOutOfRangeException("index");

            return Name;
        }

        public string GetValueTypeName(int index) {
            if (index != 0)
                throw new ArgumentOutOfRangeException("index");

            return Converter?.TargetTypeName;
        }

        public Type GetValueType(int index) {
            if (index != 0)
                throw new ArgumentOutOfRangeException("index");

            return Converter?.TargetType;
        }

        #endregion

        #region Serialization

        public JObject ToJson() {
            var obj = new JObject() {
                ["name"] = Name
            };
            if (Converter != null)
                obj["converter"] = Converter.ToJson();
            return obj;
        }

        public static SingleDataDefinition FromJson(JToken obj) {
            var converterObj = (JObject)obj["converter"];
            IValueConverter converter = null;
            if (converterObj != null) {
                var type = (string)converterObj["type"];
                if (type == "color")
                    converter = ColorConverter.FromJson(converterObj);
                else if (type == "generic")
                    converter = GenericReferenceConverter.FromJson(converterObj);
                else if (type == "icon")
                    converter = IconConverter.FromJson(converterObj);
                else if (type == "multiref")
                    converter = MultiReferenceConverter.FromJson(converterObj);
                else if (type == "link")
                    converter = SheetLinkConverter.FromJson(converterObj);
                else if (type == "tomestone")
                    converter = TomestoneOrItemReferenceConverter.FromJson(converterObj);
                else if (type == "complexlink")
                    converter = ComplexLinkConverter.FromJson(converterObj);
                else
                    throw new ArgumentException("Invalid converter type.", "obj");
            }

            return new SingleDataDefinition() {
                Name = (string)obj["name"],
                Converter = converter
            };
        }

        public void ResolveReferences(SheetDefinition sheetDef) {
            if (Converter != null)
                Converter.ResolveReferences(sheetDef);
        }

        #endregion
    }
}
