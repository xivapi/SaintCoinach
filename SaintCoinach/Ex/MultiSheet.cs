using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SaintCoinach.Ex {
    public partial class MultiSheet<TMulti, TData> : IMultiSheet<TMulti, TData>
        where TMulti : IMultiRow
        where TData : IDataRow {
        #region Fields

        private readonly ConcurrentDictionary<Language, ISheet<TData>> _LocalisedSheets =
            new ConcurrentDictionary<Language, ISheet<TData>>();

        private ConcurrentDictionary<int, TMulti> _Rows = new ConcurrentDictionary<int, TMulti>();

        #endregion

        #region Constructors

        public MultiSheet(ExCollection collection, Header header) {
            Collection = collection;
            Header = header;
        }

        #endregion

        #region Properties

        public ExCollection Collection { get; private set; }
        public Header Header { get; private set; }
        public ISheet<TData> ActiveSheet { get { return GetLocalisedSheet(Collection.ActiveLanguage); } }
        public int Count { get { return ActiveSheet.Count; } }
        public IEnumerable<int> Keys { get { return ActiveSheet.Keys; } }

        #endregion

        #region Get

        public ISheet<TData> GetLocalisedSheet(Language language) {
            return _LocalisedSheets.GetOrAdd(language, l => {
                if (!Header.AvailableLanguages.Contains(l))
                    throw new NotSupportedException();

                return CreateLocalisedSheet(l);
            });
        }

        #endregion

        #region IEnumerable<TMulti> Members

        public IEnumerator<TMulti> GetEnumerator() {
            return new Enumerator(this);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region Factory

        protected virtual TMulti CreateMultiRow(int row) {
            return (TMulti)Activator.CreateInstance(typeof(TMulti), this, row);
        }

        protected virtual ISheet<TData> CreateLocalisedSheet(Language language) {
            return new DataSheet<TData>(Collection, Header, language);
        }

        #endregion

        #region IMultiSheet<TMulti,TSource> Members

        public TMulti this[int key] {
            get {
                return _Rows.GetOrAdd(key, k => CreateMultiRow(k));
            }
        }

        #endregion

        #region ISheet Members

        public string Name { get { return Header.Name; } }

        public bool ContainsRow(int row) {
            return ActiveSheet.ContainsRow(row);
        }

        IRow ISheet.this[int row] { get { return this[row]; } }

        public object this[int row, int column] { get { return this[row][column]; } }

        #endregion

        #region IMultiSheet Members

        ISheet IMultiSheet.ActiveSheet { get { return ActiveSheet; } }

        ISheet IMultiSheet.GetLocalisedSheet(Language language) {
            return GetLocalisedSheet(language);
        }

        IMultiRow IMultiSheet.this[int row] { get { return this[row]; } }

        #endregion
    }
}
