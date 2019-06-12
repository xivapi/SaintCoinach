using Newtonsoft.Json.Linq;
using System;


namespace SaintCoinach.Ex.Relational.Definition {
    public class RepeatDataDefinition : IDataDefinition {
        #region Properties

        public int NamingOffset { get; set; }
        public int RepeatCount { get; set; }
        public IDataDefinition RepeatedDefinition { get; set; }

        #endregion

        #region Constructors

        public RepeatDataDefinition() {
            NamingOffset = 0;
        }

        #endregion

        public int Length { get { return RepeatCount * (RepeatedDefinition == null ? 0 : RepeatedDefinition.Length); } }

        public IDataDefinition Clone() {
            return new RepeatDataDefinition {
                NamingOffset = NamingOffset,
                RepeatCount = RepeatCount,
                RepeatedDefinition = RepeatedDefinition.Clone()
            };
        }

        #region IDataDefinition Members

        public object Convert(IDataRow row, object value, int index) {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException("index");

            var innerIndex = index % RepeatedDefinition.Length;

            return RepeatedDefinition.Convert(row, value, innerIndex);
        }

        public string GetName(int index) {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException("index");

            var repeatNr = index / RepeatedDefinition.Length;
            var innerIndex = index % RepeatedDefinition.Length;

            var baseName = RepeatedDefinition.GetName(innerIndex);
            return string.Format("{0}[{1}]", baseName, repeatNr + NamingOffset);
        }

        public string GetValueTypeName(int index) {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException("index");

            var innerIndex = index % RepeatedDefinition.Length;

            return RepeatedDefinition.GetValueTypeName(innerIndex);
        }

        public Type GetValueType(int index) {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException("index");

            var innerIndex = index % RepeatedDefinition.Length;

            return RepeatedDefinition.GetValueType(innerIndex);
        }

        #endregion

        #region Serialization

        public JObject ToJson() {
            return new JObject {
                ["type"] = "repeat",
                ["count"] = RepeatCount,
                ["definition"] = RepeatedDefinition.ToJson()
            };
        }

        public static RepeatDataDefinition FromJson(JToken obj) {
            return new RepeatDataDefinition() {
                RepeatCount = (int)obj["count"],
                RepeatedDefinition = DataDefinitionSerializer.FromJson(obj["definition"])
            };
        }

        public void ResolveReferences(SheetDefinition sheetDef) {
            RepeatedDefinition.ResolveReferences(sheetDef);
        }

        #endregion
    }
}
