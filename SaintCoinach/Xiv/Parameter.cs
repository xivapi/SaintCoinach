using System.Collections;
using System.Collections.Generic;

namespace SaintCoinach.Xiv {
    public class Parameter : IEnumerable<ParameterValue> {
        #region Fields

        private readonly List<ParameterValue> _Values = new List<ParameterValue>();

        #endregion

        #region Properties

        public BaseParam BaseParam { get; private set; }
        public IEnumerable<ParameterValue> Values { get { return _Values; } }

        #endregion

        #region Constructors

        #region Constructor

        public Parameter(BaseParam baseParam) {
            BaseParam = baseParam;
        }

        #endregion

        #endregion

        #region IEnumerable<ParameterValue> Members

        public IEnumerator<ParameterValue> GetEnumerator() {
            return _Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region Add

        public void AddValue(ParameterValue value) {
            _Values.Add(value);
        }

        #endregion
    }
}
