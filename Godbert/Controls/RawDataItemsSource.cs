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
        private Func<object, bool> _Filter;
        #endregion

        #region Properties
        public IComparer<object> Comparer {
            get { return _Comparer; }
            set {
                _Comparer = value;
                _Items = null;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        public Func<object, bool> Filter {
            get { return _Filter; }
            set {
                _Filter = value;
                _Items = null;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        public IEnumerable Items {
            get {
                if (_Comparer == null && _Filter == null)
                    return _Sheet;

                if (_Items == null) {
                    IEnumerable<object> results = _Sheet.Cast<object>();

                    if (_Filter != null)
                        results = results.Where(_Filter);
                    if (_Comparer != null)
                        results = results.OrderBy(o => o, Comparer);

                    _Items = results.ToArray();
                }
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
            return Items.GetEnumerator();
        }

        #endregion
    }
}
