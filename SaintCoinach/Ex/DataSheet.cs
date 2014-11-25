using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public class DataSheet<T> : IDataSheet<T> where T : IDataRow {
        #region Fields
        private ExCollection _Collection;
        private Header _Header;
        private Language _Language;
        private Dictionary<Range, ISheet<T>> _PartialSheets = new Dictionary<Range, ISheet<T>>();
        private Dictionary<int, ISheet<T>> _RowToPartialSheetMap = new Dictionary<int, ISheet<T>>();
        private T[] _AllRows = null;
        #endregion

        #region Properties
        public ExCollection Collection { get { return _Collection; } }
        public Header Header { get { return _Header; } }
        public Language Language { get { return _Language; } }
        #endregion

        #region Constructor
        public DataSheet(ExCollection collection, Header header, Language language) {
            _Collection = collection;
            _Header = header;
            _Language = language;
        }
        #endregion

        #region Factory
        protected virtual ISheet<T> CreatePartialSheet(Range range, IO.File file) {
            return new PartialDataSheet<T>(this, range, file);
        }
        #endregion

        #region Helpers
        protected IO.File GetPartialFile(Range range) {
            const string PartialFileNameFormat = "exd/{0}_{1}{2}.exd";

            var partialFileName = string.Format(PartialFileNameFormat, Header.Name, range.Start, Language.GetSuffix());
            var file = Collection.PackCollection.GetFile(partialFileName);

            return file;
        }
        protected ISheet<T> GetPartialSheet(int row) {
            if (_RowToPartialSheetMap.ContainsKey(row))
                return _RowToPartialSheetMap[row];

            var res = Header.DataFileRanges.Where(_ => _.Contains(row));
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
            foreach (var range in Header.DataFileRanges) {
                if (!_PartialSheets.ContainsKey(range))
                    CreatePartialSheet(range);
            }
        }
        private ISheet<T> CreatePartialSheet(Range range) {
            var file = GetPartialFile(range);

            var partial = CreatePartialSheet(range, file);
            _PartialSheets.Add(range, partial);
            foreach (var row in partial.GetAllRows())
                _RowToPartialSheetMap.Add(row.Key, partial);
            return partial;
        }
        #endregion

        #region ISheet<T> Members
        public IEnumerable<T> GetAllRows() {
            CreateAllPartialSheets();

            if (_AllRows == null) {
                var rows = new List<T>();

                foreach (var partial in _PartialSheets.Values)
                    rows.AddRange(partial.GetAllRows());

                _AllRows = rows.ToArray();
            }

            return _AllRows;
        }

        public T this[int row] {
            get { return GetPartialSheet(row)[row]; }
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator() {
            // XXX: Make a proper enumerator
            return GetAllRows().GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region ISheet Members

        public string Name {
            get { return Header.Name + "_" + Language.GetCode(); }
        }

        public bool ContainsRow(int row) {
            return Range.Contains(Header.DataFileRanges, row);
        }

        IEnumerable<IRow> ISheet.GetAllRows() {
            return GetAllRows().Cast<IRow>();
        }

        IRow ISheet.this[int row] {
            get { return (IRow)this[row]; }
        }

        public object this[int row, int column] {
            get { return this[row][column]; }
        }

        #endregion

        #region IDataSheet Members
        public byte[] GetBuffer() {
            throw new NotSupportedException();
        }
        #endregion
    }
}
