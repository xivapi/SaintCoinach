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
                if (_Items != null)
                    return Items;

                if (_Sheet.Header.Variant == 1)
                    return GetVariant1Items();
                else
                    return GetVariant2Items();
            }
        }

        private IEnumerable GetVariant1Items()
        {
            if (_Comparer == null && _Filter == null)
                return _Sheet;

            _Items = FilterAndCompare(_Sheet.Cast<object>()).ToArray();
            return _Items;
        }

        private IEnumerable GetVariant2Items()
        {
            var rows = _Sheet.Cast<SaintCoinach.Xiv.XivSubRow>();
            _Items = FilterAndCompare(rows).ToArray();
            return _Items;
        }

        private IEnumerable<object> FilterAndCompare(IEnumerable<object> results)
        {
            if (_Filter != null)
                results = results.Where(_Filter);
            if (_Comparer != null)
                results = results.OrderBy(o => o, Comparer);
            return results;
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
