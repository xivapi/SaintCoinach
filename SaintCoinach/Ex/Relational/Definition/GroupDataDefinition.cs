using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Definition {
    public class GroupDataDefinition : IDataDefinition {
        #region Fields
        private ICollection<IDataDefinition> _Members = new List<IDataDefinition>();
        #endregion

        #region Properties
        public ICollection<IDataDefinition> Members {
            get { return _Members; }
            internal set { _Members = value; }
        }
        [YamlDotNet.Serialization.YamlIgnore]
        public int Length {
            get { return _Members.Sum(_ => _.Length); }
        }
        #endregion

        #region IDataDefinition Members
        public object Convert(IDataRow row, object value, int index) {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException("index");

            object convertedValue = value;
            var pos = 0;
            foreach (var member in Members) {
                var newPos = pos + member.Length;
                if (newPos > index) {
                    var innerIndex = index - pos;

                    convertedValue = member.Convert(row, value, innerIndex);

                    break;
                }
                pos = newPos;
            }
            return convertedValue;
        }

        public string GetName(int index) {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException("index");

            string value = null;
            var pos = 0;
            foreach (var member in Members) {
                var newPos = pos + member.Length;
                if (newPos > index) {
                    var innerIndex = index - pos;
                    value = member.GetName(innerIndex);
                    break;
                }
                pos = newPos;
            }
            return value;
        }

        public string GetValueType(int index) {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException("index");

            string value = null;
            var pos = 0;
            foreach (var member in Members) {
                var newPos = pos + member.Length;
                if (newPos > index) {
                    var innerIndex = index - pos;
                    value = member.GetValueType(innerIndex);
                    break;
                }
                pos = newPos;
            }
            return value;
        }
        #endregion

        public IDataDefinition Clone() {
            var clone = new GroupDataDefinition();

            foreach (var member in this.Members)
                clone.Members.Add(member.Clone());

            return clone;
        }
    }
}
