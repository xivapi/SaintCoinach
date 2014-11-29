using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Parameter : IEnumerable<ParameterValue> {
        #region Fields
        private BaseParam _BaseParam;
        private List<ParameterValue> _Values = new List<ParameterValue>();
        #endregion

        #region Properties
        public BaseParam BaseParam { get { return _BaseParam; } }
        public IEnumerable<ParameterValue> Values { get { return _Values; } }
        #endregion

        #region Constructor
        public Parameter(BaseParam baseParam) {
            _BaseParam = baseParam;
        }
        #endregion

        #region Add
        public void AddValue(ParameterValue value) {
            _Values.Add(value);
        }
        #endregion

        #region IEnumerable<ParameterValue> Members

        public IEnumerator<ParameterValue> GetEnumerator() {
            return _Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
