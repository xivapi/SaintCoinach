using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Data {
    public class ParametersBase : ICloneable {
        #region Fields
        private Dictionary<string, object> _Entries = new Dictionary<string, object>();
        #endregion

        #region Constructor
        public ParametersBase() { }
        public ParametersBase(ParametersBase copyFrom) {
            this._Entries = new Dictionary<string, object>(copyFrom._Entries);
        }
        #endregion

        #region Get/Set
        public void Set<T>(string key, T value) {
            if (_Entries.ContainsKey(key))
                _Entries[key] = value;
            else
                _Entries.Add(key, value);
        }
        public bool Contains(string key) {
            return _Entries.ContainsKey(key);
        }
        public bool Contains<T>(string key) {
            return _Entries.ContainsKey(key) && _Entries[key] is T;
        }
        public bool Remove(string key) {
            return _Entries.Remove(key);
        }
        public T Get<T>(string key) {
            return (T)_Entries[key];
        }
        public bool TryGetValue<T>(string key, out T value) {
            value = default(T);
            object asObj;
            if (!_Entries.TryGetValue(key, out asObj) || !(asObj is T))
                return false;
            value = (T)asObj;
            return true;
        }
        #endregion

        #region Clone
        public virtual object Clone() {
            return new ParametersBase(this);
        }
        #endregion
    }
}
