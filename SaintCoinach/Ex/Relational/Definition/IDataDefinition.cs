using Newtonsoft.Json.Linq;
using System;
using SaintCoinach.Ex.Relational.Definition.EXDSchema;
using Field = SaintCoinach.Ex.Relational.Definition.EXDSchema.Field;

namespace SaintCoinach.Ex.Relational.Definition {
    public interface IDataDefinition {
        #region Properties

        int Length { get; }

        #endregion

        /// <param name="row">The row to convert a value of.</param>
        /// <param name="value">Raw value as read from the file.</param>
        /// <param name="index">Index in the definition on which the method is called.</param>
        object Convert(IDataRow row, object value, int index);

        /// <param name="index">Index in the definition on which the method is called.</param>
        string GetName(int index);

        /// <param name="index">Index in the definition on which the method is called.</param>
        string GetValueTypeName(int index);

        /// <param name="index">Index in the definition on which the method is called.</param>
        Type GetValueType(int index);

        IDataDefinition Clone();

        void ResolveReferences(SheetDefinition sheetDef);
    }

    public static class DataDefinitionSerializer {
        public static IDataDefinition FromJson(JToken obj) {
            var type = (string)obj["type"];
            if (type == null)
                return SingleDataDefinition.FromJson(obj);
            else if (type == "group")
                return GroupDataDefinition.FromJson(obj);
            else if (type == "repeat")
                return RepeatDataDefinition.FromJson(obj);
            else
                throw new ArgumentException("Invalid definition type.", "obj");
        }
        
        public static IDataDefinition FromYaml(Field field, bool isFromRepeat = false)
        {
            switch (field.Type)
            {
                case FieldType.Array:
                    if (isFromRepeat) {
                        if (field.Fields == null || field.Fields.Count == 1)
                            return SingleDataDefinition.FromYaml(field);
                        if (field.Fields.Count > 1)
                            return GroupDataDefinition.FromYaml(field);
                    }
                    return RepeatDataDefinition.FromYaml(field);
                case FieldType.Scalar:
                case FieldType.Icon:
                case FieldType.ModelId:
                case FieldType.Color:
                case FieldType.Link:
                    return SingleDataDefinition.FromYaml(field);
                default:
                    throw new ArgumentException("Invalid definition type.", "obj");
            }
        }
    }
}
