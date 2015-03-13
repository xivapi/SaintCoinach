using System;

using YamlDotNet.Serialization;

namespace SaintCoinach.Ex.Relational.Definition {
    public class SingleDataDefinition : IDataDefinition {
        #region Properties

        public string Name { get; set; }
        public IValueConverter Converter { get; set; }

        #endregion

        [YamlIgnore]
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

            return Converter == null ? null : Converter.TargetTypeName;
        }

        public Type GetValueType(int index) {
            if (index != 0)
                throw new ArgumentOutOfRangeException("index");

            return Converter == null ? null : Converter.TargetType;
        }

        #endregion
    }
}
