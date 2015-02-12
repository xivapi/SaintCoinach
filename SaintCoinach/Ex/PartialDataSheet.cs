using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.IO;

namespace SaintCoinach.Ex {
    public class PartialDataSheet<T> : IDataSheet<T> where T : IDataRow {
        #region Fields

        private Dictionary<int, T> _Rows;

        #endregion

        #region Properties

        public IDataSheet<T> SourceSheet { get; private set; }
        public Range Range { get; private set; }
        public File File { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public PartialDataSheet(IDataSheet<T> sourceSheet, Range range, File file) {
            SourceSheet = sourceSheet;
            Range = range;
            File = file;

            Build();
        }

        #endregion

        #endregion

        public Language Language { get { return SourceSheet.Language; } }
        public int Count { get { return _Rows.Count; } }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator() {
            return _Rows.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region IDataSheet Members

        public byte[] GetBuffer() {
            return File.GetData();
        }

        #endregion

        #region Build

        private void Build() {
            const int HeaderLengthOffset = 0x08;

            const int EntriesOffset = 0x20;
            const int EntryLength = 0x08;

            const int EntryKeyOffset = 0x00;
            const int EntryPositionOffset = 0x04;

            var buffer = File.GetData();

            var headerLen = OrderedBitConverter.ToInt32(buffer, HeaderLengthOffset, true);
            var count = headerLen / EntryLength;
            var currentPosition = EntriesOffset;

            _Rows = new Dictionary<int, T>();
            for (var i = 0; i < count; ++i) {
                var key = OrderedBitConverter.ToInt32(buffer, currentPosition + EntryKeyOffset, true);
                var off = OrderedBitConverter.ToInt32(buffer, currentPosition + EntryPositionOffset, true);
                _Rows.Add(key, CreateRow(key, off));
                currentPosition += EntryLength;
            }
        }

        #endregion

        #region Factory

        protected virtual T CreateRow(int key, int offset) {
            return (T)Activator.CreateInstance(typeof(T), this, key, offset);
        }

        #endregion

        #region ISheet<T> Members

        public IEnumerable<T> GetAllRows() {
            return _Rows.Values;
        }

        public T this[int row] { get { return _Rows[row]; } }

        #endregion

        #region ISheet Members

        public string Name { get { return SourceSheet.Name + "_" + Range.Start; } }

        public Header Header { get { return SourceSheet.Header; } }

        public ExCollection Collection { get { return SourceSheet.Collection; } }

        public bool ContainsRow(int row) {
            return _Rows.ContainsKey(row);
        }

        IEnumerable<IRow> ISheet.GetAllRows() {
            return _Rows.Values.Cast<IRow>();
        }

        IRow ISheet.this[int row] { get { return this[row]; } }

        public object this[int row, int column] { get { return this[row][column]; } }

        #endregion
    }
}
