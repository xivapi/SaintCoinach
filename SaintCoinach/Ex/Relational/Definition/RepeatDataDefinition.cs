using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Definition {
    public class RepeatDataDefinition : IDataDefinition {
        #region Fields
        private int _RepeatCount;
        private int _NamingOffset = 0;
        private IDataDefinition _RepeatedDefinition;
        #endregion

        #region Properties
        [YamlDotNet.Serialization.YamlIgnore]
        public int Length {
            get { return RepeatCount * (RepeatedDefinition == null ? 0 : RepeatedDefinition.Length); }
        }
        public int NamingOffset {
            get { return _NamingOffset; }
            set { _NamingOffset = value; }
        }
        public int RepeatCount {
            get { return _RepeatCount; }
            set { _RepeatCount = value; }
        }
        public IDataDefinition RepeatedDefinition {
            get { return _RepeatedDefinition; }
            set { _RepeatedDefinition = value; }
        }
        #endregion

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
        public string GetValueType(int index) {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException("index");

            var innerIndex = index % RepeatedDefinition.Length;

            return RepeatedDefinition.GetValueType(innerIndex);
        }
        #endregion

        public IDataDefinition Clone() {
            return new RepeatDataDefinition() {
                NamingOffset = this.NamingOffset,
                RepeatCount = this.RepeatCount,
                RepeatedDefinition = this.RepeatedDefinition.Clone()
            };
        }
    }
}
