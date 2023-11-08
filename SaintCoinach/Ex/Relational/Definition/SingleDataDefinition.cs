using Newtonsoft.Json.Linq;
using SaintCoinach.Ex.Relational.ValueConverters;
using System;
using Field = SaintCoinach.Ex.Relational.Definition.EXDSchema.Field;
using FieldType = SaintCoinach.Ex.Relational.Definition.EXDSchema.FieldType;

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

        public static SingleDataDefinition FromJson(JToken obj) {
            var converterObj = (JObject)obj["converter"];
            IValueConverter converter = null;
            if (converterObj != null) {
                var type = (string)converterObj["type"];
                if (type == "color")
                    converter = new ColorConverter();
                else if (type == "generic")
                    converter = GenericReferenceConverter.FromJson(converterObj);
                else if (type == "icon")
                    converter = new IconConverter();
                else if (type == "multiref")
                    converter = MultiReferenceConverter.FromJson(converterObj);
                else if (type == "link")
                    converter = SheetLinkConverter.FromJson(converterObj);
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
        
        public static IDataDefinition FromYaml(Field field)
        {
            IValueConverter converter = null;

            switch (field.Type)
            {
                case FieldType.Array:
                case FieldType.Scalar:
                    break;
                case FieldType.Icon:
                    converter = new IconConverter();
                    break;
                case FieldType.ModelId:
                    // TODO
                    break;
                case FieldType.Color:
                    converter = new ColorConverter();
                    break;
                case FieldType.Link:
                    if (field.Condition == null)
                    {
                        // Single or multi
                        if (field.Targets.Count == 1) 
                            converter = SheetLinkConverter.FromYaml(field.Targets[0]);
                        else
                            converter = MultiReferenceConverter.FromYaml(field.Targets);
                    }
                    else
                    {
                        converter = ComplexLinkConverter.FromYaml(field.Condition);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new SingleDataDefinition()
            {
                Name = field.Name,
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
