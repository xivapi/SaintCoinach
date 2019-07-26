using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SaintCoinach.Ex;
using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public partial class XivSheet<T> : IXivSheet<T> where T : IXivRow {
        #region Fields

        protected readonly ConcurrentDictionary<int, T> _Rows = new ConcurrentDictionary<int, T>();
        protected readonly IRelationalSheet _Source;
        private ConstructorInfo _RowConstructor;

        #endregion

        #region Properties

        private ConstructorInfo RowConstructor {
            get {
                if (_RowConstructor != null) return _RowConstructor;
                return _RowConstructor = GetRowConstructor(typeof(T), GetType());
            }
        }

        protected static ConstructorInfo GetRowConstructor(Type type, Type sheetType) {
            var ctors =
                type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var ctor in ctors) {
                var param = ctor.GetParameters();
                if (param.Length != 2) continue;
                if (!param[0].ParameterType.IsAssignableFrom(sheetType)) continue;
                if (!param[1].ParameterType.IsAssignableFrom(typeof(IRelationalRow))) continue;

                return ctor;
            }

            throw new NotSupportedException("No matching constructor found.");
        }

        public IEnumerable<int> Keys { get { return _Source.Keys; } }

        #endregion

        #region Constructors

        public XivSheet(XivCollection collection, IRelationalSheet source) {
            Collection = collection;
            _Source = source;
        }

        #endregion

        public XivCollection Collection { get; private set; }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator() {
            return new Enumerator(this);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region Factory

        protected virtual T CreateRow(IRelationalRow sourceRow) {
            if (RowConstructor == null)
                throw new NotSupportedException("No matching constructor found.");

            return (T)RowConstructor.Invoke(new object[] {
                this, sourceRow
            });
        }

        #endregion

        #region ISheet<T> Members

        public T this[int key] {
            get {
                return _Rows.GetOrAdd(key, k => {
                    if (!_Source.ContainsRow(k))
                        throw new KeyNotFoundException();

                    return CreateRow(_Source[key]);
                });
            }
        }

        #endregion

        #region ISheet Members

        string ISheet.Name { get { return _Source.Name; } }

        Header ISheet.Header { get { return _Source.Header; } }

        ExCollection ISheet.Collection { get { return Collection; } }

        public int Count { get { return _Source.Count; } }

        public bool ContainsRow(int row) {
            return _Source.ContainsRow(row);
        }

        IRow ISheet.this[int row] { get { return this[row]; } }

        public object this[int row, int column] { get { return this[row][column]; } }

        #endregion

        #region IRelationalSheet Members

        RelationalHeader IRelationalSheet.Header { get { return _Source.Header; } }

        RelationalExCollection IRelationalSheet.Collection { get { return Collection; } }

        IRelationalRow IRelationalSheet.this[int row] { get { return this[row]; } }

        public object this[int row, string columnName] { get { return this[row][columnName]; } }

        public IRelationalRow IndexedLookup(string index, int key) { return _Source.IndexedLookup(index, key); }

        #endregion

        #region IXivSheet Members

        IXivRow IXivSheet.this[int row] { get { return this[row]; } }

        #endregion
    }
}
