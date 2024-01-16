using Newtonsoft.Json.Linq;
using System;
using Field = SaintCoinach.Ex.Relational.Definition.EXDSchema.Field;

namespace SaintCoinach.Ex.Relational.Definition {
    public class PositionedDataDefinition {
        #region Properties

        public IDataDefinition InnerDefinition { get; set; }

        public int Length { get { return InnerDefinition == null ? 0 : InnerDefinition.Length; } }

        public int ColumnBasedIndex { get; set; }
        public int OffsetBasedIndex { get; private set; }

        #endregion

        public PositionedDataDefinition Clone() {
            var clone = new PositionedDataDefinition {
                ColumnBasedIndex = ColumnBasedIndex,
                OffsetBasedIndex = OffsetBasedIndex,
                InnerDefinition = InnerDefinition.Clone()
            };
            
            return clone;
        }

        #region Things

        public object Convert(IDataRow row, object value, int index) {
            var innerIndex = index - ColumnBasedIndex;
            if (innerIndex < 0 || innerIndex >= Length)
                throw new ArgumentOutOfRangeException("index");

            return InnerDefinition.Convert(row, value, innerIndex);
        }

        public string GetName(int index) {
            var innerIndex = index - ColumnBasedIndex;
            if (innerIndex < 0 || innerIndex >= Length)
                throw new ArgumentOutOfRangeException("index");

            return InnerDefinition.GetName(innerIndex);
        }

        public string GetValueTypeName(int index) {
            var innerIndex = index - ColumnBasedIndex;
            if (innerIndex < 0 || innerIndex >= Length)
                throw new ArgumentOutOfRangeException("index");

            return InnerDefinition.GetValueTypeName(innerIndex);
        }

        public Type GetValueType(int index) {
            var innerIndex = index - ColumnBasedIndex;
            if (innerIndex < 0 || innerIndex >= Length)
                throw new ArgumentOutOfRangeException("index");

            return InnerDefinition.GetValueType(innerIndex);
        }

        #endregion

        #region Serialization

        public static PositionedDataDefinition FromJson(JToken obj) {
            return new PositionedDataDefinition() {
                ColumnBasedIndex = (int?)obj["index"] ?? 0,
                InnerDefinition = DataDefinitionSerializer.FromJson(obj)
            };
        }
        
        public static PositionedDataDefinition FromYaml(Field field) {
            return new PositionedDataDefinition() {
                OffsetBasedIndex = field.Index,
                InnerDefinition = DataDefinitionSerializer.FromYaml(field)
            };
        }

        public void ResolveReferences(SheetDefinition sheetDef) {
            InnerDefinition.ResolveReferences(sheetDef);
        }

        #endregion
    }
}
