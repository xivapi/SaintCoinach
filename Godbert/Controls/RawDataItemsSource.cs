using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace Godbert.Controls {
    public class RawDataItemsSource : IEnumerable, INotifyCollectionChanged {
        #region Fields
        private IRelationalSheet _Sheet;
        private IComparer<object> _Comparer;
        private object[] _Items;
        #endregion

        #region Properties
        public IComparer<object> Comparer {
            get { return _Comparer; }
            set {
                _Comparer = value;
                _Items = null;
                var h = CollectionChanged;
                if (h != null)
                    h(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        public IEnumerable Items {
            get {
                if (Comparer == null)
                    return _Sheet;
                if (_Items == null)
                    _Items = _Sheet.Cast<object>().OrderBy(o => o, Comparer).ToArray();
                return _Items;
            }
        }
        #endregion

        #region Constructor
        public RawDataItemsSource(IRelationalSheet sheet) {
            _Sheet = sheet;
        }
        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator() {
            var items = Items;
            if (items == null)
                return (new object[0]).GetEnumerator();
            return items.GetEnumerator();
        }

        #endregion
    }
}
