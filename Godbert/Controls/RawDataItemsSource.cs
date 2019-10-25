using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaintCoinach.Ex;
using SaintCoinach.Ex.Relational;

namespace Godbert.Controls {
    public class RawDataItemsSource : IEnumerable, INotifyCollectionChanged {
        #region Fields
        private IRelationalSheet _Sheet;
        private IComparer<object> _Comparer;
        private object[] _Items;
        private string _Filter;
        private Tuple<IRow, string>[] _RowSearchIndex;
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
        public string Filter {
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
            if (_Filter != null) {
                var rows = results.OfType<IRow>().ToArray();
                if (_RowSearchIndex == null)
                    BuildSearchIndex(rows);
                results = FilterMatchingRows();
            }
            if (_Comparer != null)
                results = results.OrderBy(o => o, Comparer);
            return results;
        }

        #endregion

        #region Filter

        private void BuildSearchIndex(IRow[] rows) {
            if (rows.Length == 0)
                return;

            var start = DateTime.Now;
            System.Diagnostics.Debug.WriteLine($"Rebuilding search index for {_Sheet.Name}...");

            var newIndex = new ConcurrentBag<Tuple<IRow, string>>();
            var columns = rows[0].Sheet.Header.Columns.ToArray();

            Parallel.ForEach(rows, row => {
                var index = new StringBuilder();
                index.Append(row.Key.ToString());
                index.Append("||");

                foreach (var value in row.ColumnValues()) {
                    if (value != null) {
                        index.Append(value.ToString());
                        index.Append("||");
                    }
                }

                newIndex.Add(Tuple.Create(row, index.ToString()));
            });

            _RowSearchIndex = newIndex.OrderBy(r => r.Item1.Key).ToArray();
            System.Diagnostics.Debug.WriteLine($"{_Sheet.Name} search index complete.  Elapsed {DateTime.Now - start}.");
        }

        private IRow[] FilterMatchingRows() {
            return _RowSearchIndex
                .Where(r => r.Item2.IndexOf(_Filter, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(r => r.Item1)
                .ToArray();
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
