using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.IO;

namespace SaintCoinach.Ex {
    public partial class DataSheet<T> : IDataSheet<T> where T : IDataRow {
        #region Fields

        private bool _PartialSheetsCreated = false;
        private readonly Dictionary<Range, ISheet<T>> _PartialSheets = new Dictionary<Range, ISheet<T>>();
        private readonly Dictionary<int, ISheet<T>> _RowToPartialSheetMap = new Dictionary<int, ISheet<T>>();

        #endregion

        #region Constructors

        public DataSheet(ExCollection collection, Header header, Language language) {
            Collection = collection;
            Header = header;
            Language = language;
        }

        #endregion

        public ExCollection Collection { get; private set; }
        public Header Header { get; private set; }
        public Language Language { get; private set; }

        public int Count {
            get {
                CreateAllPartialSheets();
                return _PartialSheets.Values.Sum(_ => _.Count);
            }
        }

        public IEnumerable<int> Keys {
            get {
                CreateAllPartialSheets();
                return _RowToPartialSheetMap.Keys;
            }
        }

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

        #region IDataSheet Members

        public byte[] GetBuffer() {
            throw new NotSupportedException();
        }

        #endregion

        #region Factory

        protected virtual ISheet<T> CreatePartialSheet(Range range, File file) {
            return new PartialDataSheet<T>(this, range, file);
        }

        #endregion

        #region Helpers

        protected File GetPartialFile(Range range) {
            const string PartialFileNameFormat = "exd/{0}_{1}{2}.exd";

            var partialFileName = string.Format(PartialFileNameFormat, Header.Name, range.Start, Language.GetSuffix());
            var file = Collection.PackCollection.GetFile(partialFileName);

            return file;
        }

        protected ISheet<T> GetPartialSheet(int row) {
            if (_RowToPartialSheetMap.ContainsKey(row))
                return _RowToPartialSheetMap[row];

            var res = Header.DataFileRanges.Where(_ => _.Contains(row)).ToArray();
            if (!res.Any())
                throw new ArgumentOutOfRangeException();

            ISheet<T> partial;
            var range = res.First();
            if (!_PartialSheets.TryGetValue(range, out partial)) {
                partial = CreatePartialSheet(range);
            }
            return partial;
        }

        private void CreateAllPartialSheets() {
            if (_PartialSheetsCreated)
                return;
            foreach (var range in Header.DataFileRanges.Where(range => !_PartialSheets.ContainsKey(range))) {
                CreatePartialSheet(range);
            }
            _PartialSheetsCreated = true;
        }

        private ISheet<T> CreatePartialSheet(Range range) {
            var file = GetPartialFile(range);

            var partial = CreatePartialSheet(range, file);
            _PartialSheets.Add(range, partial);
            foreach (var key in partial.Keys)
                _RowToPartialSheetMap.Add(key, partial);
            return partial;
        }

        #endregion

        #region ISheet<T> Members

        public T this[int row] { get { return GetPartialSheet(row)[row]; } }

        #endregion

        #region ISheet Members

        public string Name { get { return Header.Name + Language.GetSuffix(); } }

        public bool ContainsRow(int row) {
            CreateAllPartialSheets();

            return _RowToPartialSheetMap.ContainsKey(row);
        }

        IRow ISheet.this[int row] { get { return this[row]; } }

        public object this[int row, int column] { get { return this[row][column]; } }

        #endregion
    }
}
