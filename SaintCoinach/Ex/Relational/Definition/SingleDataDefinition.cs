using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Definition {
    public class SingleDataDefinition : IDataDefinition {
        #region Fields
        private string _Name;
        private IValueConverter _Converter;
        #endregion

        #region Properties
        [YamlDotNet.Serialization.YamlIgnore]
        public int Length {
            get { return 1; }
        }
        public string Name {
            get { return _Name; }
            set { _Name = value; }
        }
        public IValueConverter Converter {
            get { return _Converter; }
            set { _Converter = value; }
        }
        #endregion

        #region IDataDefinition Members
        public object Convert(IDataRow row, object value, int index) {
            if (index != 0)
                throw new ArgumentOutOfRangeException("index");

            if (Converter != null)
                return Converter.Convert(row, value);
            return value;
        }

        public string GetName(int index) {
            if (index != 0)
                throw new ArgumentOutOfRangeException("index");

            return Name;
        }

        public string GetValueType(int index) {
            if (index != 0)
                throw new ArgumentOutOfRangeException("index");

            if (Converter == null)
                return null;
            return Converter.TargetTypeName;
        }
        #endregion

        public IDataDefinition Clone() {
            return new SingleDataDefinition() {
                Name = this.Name,
                Converter = this.Converter
            };
        }
    }
}
