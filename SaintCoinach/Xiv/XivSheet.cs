using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    using Ex.Relational;

    public class XivSheet<T> : IXivSheet<T> where T : IXivRow {
        #region Fields
        private bool _RowsFilled = false;
        private IRelationalSheet _Source;
        private Dictionary<int, T> _Rows = new Dictionary<int, T>();
        private ConstructorInfo _RowConstructor;
        private XivCollection _Collection;
        #endregion

        #region Properties
        public XivCollection Collection { get { return _Collection; } }
        #endregion

        #region Constructor
        public XivSheet(XivCollection collection, IRelationalSheet source) {
            _Collection = collection;
            _Source = source;
        }
        #endregion

        #region Factory
        private void FillRows() {
            if (_RowsFilled)
                return;

            foreach (IRelationalRow sourceRow in _Source) {
                if (!_Rows.ContainsKey(sourceRow.Key)) {
                    var row = CreateRow(sourceRow);
                    _Rows.Add(sourceRow.Key, row);
                }
            }

            _RowsFilled = true;
        }
        private T Get(int key) {
            T val;
            if (!_Rows.TryGetValue(key, out val)) {
                if (!_Source.ContainsRow(key))
                    throw new KeyNotFoundException();

                var sourceRow = _Source[key];
                val = CreateRow(sourceRow);
                _Rows.Add(key, val);
            }
            return val;
        }
        protected virtual T CreateRow(IRelationalRow sourceRow) {
            if (_RowConstructor == null) {
                var ctors = typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (var ctor in ctors) {
                    var param = ctor.GetParameters();
                    if (param.Length != 2)
                        continue;
                    if (!param[0].ParameterType.IsAssignableFrom(this.GetType()))
                        continue;
                    if (!param[1].ParameterType.IsAssignableFrom(typeof(IRelationalRow)))
                        continue;

                    _RowConstructor = ctor;
                    break;
                }

                if (_RowConstructor == null)
                    throw new NotSupportedException("No matching constructor found.");
            }

            return (T)_RowConstructor.Invoke(new object[] { this, sourceRow });
        }
        #endregion

        #region ISheet<T> Members

        public IEnumerable<T> GetAllRows() {
            FillRows();

            return _Rows.Values;
        }

        public T this[int row] {
            get { return Get(row); }
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator() {
            return GetAllRows().GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region ISheet Members

        string Ex.ISheet.Name {
            get { return _Source.Name; }
        }

        Ex.Header Ex.ISheet.Header {
            get { return _Source.Header; }
        }

        Ex.ExCollection Ex.ISheet.Collection {
            get { return Collection; }
        }

        public int Count {
            get { return _Source.Count; }
        }

        public bool ContainsRow(int row) {
            return _Source.ContainsRow(row);
        }

        IEnumerable<Ex.IRow> Ex.ISheet.GetAllRows() {
            return GetAllRows().Cast<Ex.IRow>();
        }

        Ex.IRow Ex.ISheet.this[int row] {
            get { return this[row]; }
        }

        public object this[int row, int column] {
            get { return this[row][column]; }
        }

        #endregion

        #region IRelationalSheet Members

        RelationalHeader IRelationalSheet.Header {
            get { return _Source.Header; }
        }

        RelationalExCollection IRelationalSheet.Collection {
            get { return Collection; }
        }

        IEnumerable<IRelationalRow> IRelationalSheet.GetAllRows() {
            return GetAllRows().Cast<IRelationalRow>();
        }

        IRelationalRow IRelationalSheet.this[int row] {
            get { return this[row]; }
        }

        public object this[int row, string columnName] {
            get { return this[row][columnName]; }
        }

        #endregion

        #region IXivSheet Members
        IEnumerable<IXivRow> IXivSheet.GetAllRows() {
            return GetAllRows().Cast<IXivRow>();
        }

        IXivRow IXivSheet.this[int row] {
            get { return this[row]; }
        }

        #endregion
    }
}
