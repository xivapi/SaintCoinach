using System.Collections;
using System.Collections.Generic;

namespace SaintCoinach.Xiv {
    public class ParameterCollection : IEnumerable<Parameter> {
        #region Fields

        private readonly Dictionary<BaseParam, Parameter> _Parameters = new Dictionary<BaseParam, Parameter>();

        #endregion

        #region Properties

        public IEnumerable<Parameter> Parameters { get { return _Parameters.Values; } }

        #endregion

        #region Add

        public void AddParameterValue(BaseParam baseParam, ParameterValue value) {
            Parameter param;
            if (!_Parameters.TryGetValue(baseParam, out param))
                _Parameters.Add(baseParam, param = new Parameter(baseParam));

            param.AddValue(value);
        }

        public void AddRange(IEnumerable<Parameter> other) {
            foreach (var p in other) {
                foreach (var v in p)
                    AddParameterValue(p.BaseParam, v);
            }
        }

        #endregion

        #region IEnumerable<Parameter> Members

        public IEnumerator<Parameter> GetEnumerator() {
            return _Parameters.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
