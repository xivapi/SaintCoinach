using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Definition {
    public class PositionedDataDefintion {
        #region Fields
        private IDataDefinition _InnerDefintion;
        private int _Index;
        #endregion

        #region Properties
        public IDataDefinition InnerDefinition {
            get { return _InnerDefintion; }
            set { _InnerDefintion = value; }
        }
        [YamlDotNet.Serialization.YamlIgnore]
        public int Length {
            get { return InnerDefinition == null ? 0 : InnerDefinition.Length; }
        }
        public int Index {
            get { return _Index; }
            set { _Index = value; }
        }
        #endregion

        #region Things
        public object Convert(IDataRow row, object value, int index) {
            var innerIndex = index - this.Index;
            if (innerIndex < 0 || innerIndex >= Length)
                throw new ArgumentOutOfRangeException("offset");

            return InnerDefinition.Convert(row, value, innerIndex);
        }
        public string GetName(int index) {
            var innerIndex = index - this.Index;
            if (innerIndex < 0 || innerIndex >= Length)
                throw new ArgumentOutOfRangeException("offset");

            return InnerDefinition.GetName(innerIndex);
        }
        public string GetValueType(int index) {
            var innerIndex = index - this.Index;
            if (innerIndex < 0 || innerIndex >= Length)
                throw new ArgumentOutOfRangeException("offset");

            return InnerDefinition.GetValueType(innerIndex);
        }
        #endregion

        public PositionedDataDefintion Clone() {
            var clone = new PositionedDataDefintion();

            clone.Index = this.Index;
            clone.InnerDefinition = this.InnerDefinition.Clone();

            return clone;
        }
    }
}
