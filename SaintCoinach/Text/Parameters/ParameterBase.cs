using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Parameters {
    public class ParameterBase {
        #region Fields
        private object _FallbackValue = null;
        private Dictionary<int, object> _Values;
        #endregion

        #region Properties
        public object FallbackValue { get { return _FallbackValue; } set { _FallbackValue = value; } }
        #endregion

        #region Constructors
        public ParameterBase() {
            _Values = new Dictionary<int, object>();
        }
        public ParameterBase(ParameterBase copyFrom) {
            if (copyFrom == null)
                throw new ArgumentNullException("copyFrom");
            _FallbackValue = copyFrom._FallbackValue;
            _Values = new Dictionary<int, object>(copyFrom._Values);
        }
        #endregion

        public void Clear() {
            _Values.Clear();
        }
        public bool Remove(int index) {
            return _Values.Remove(index);
        }

        #region Indexer
        public object this[int index] {
            get {
                object v;
                if (!_Values.TryGetValue(index, out v))
                    v = FallbackValue;
                return v;
            }
            set {
                if (_Values.ContainsKey(index))
                    _Values[index] = value;
                else
                    _Values.Add(index, value);
            }
        }
        #endregion
    }
}
