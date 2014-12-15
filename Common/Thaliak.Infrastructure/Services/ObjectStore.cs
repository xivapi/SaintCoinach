using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;

namespace Thaliak.Services {
    [Export(typeof(IObjectStore))]
    public class ObjectStore : IObjectStore {
        private Dictionary<Guid, object> _Store = new Dictionary<Guid,object>();
        private Dictionary<object, Guid> _IdMap = new Dictionary<object, Guid>();

        #region ITemporaryStorage Members

        public Guid Store(object value) {
            if (value == null)
                throw new ArgumentNullException("value");

            if (_IdMap.ContainsKey(value))
                return _IdMap[value];

            var id = Guid.NewGuid();
            _Store.Add(id, value);
            _IdMap.Add(value, id);
            return id;
        }

        public object Retrieve(Guid id) {
            if (_Store.ContainsKey(id))
                return _Store[id];
            return null;
        }
        #endregion
    }
}
