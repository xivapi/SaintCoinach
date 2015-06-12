using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.IO;

namespace SaintCoinach.Ex {
    public partial class PartialDataSheet<T> : IDataSheet<T> where T : IDataRow {
        #region Fields

        private Dictionary<int, T> _Rows;
        private Dictionary<int, int> _RowOffsets = new Dictionary<int, int>();

        #endregion

        #region Properties

        public IDataSheet<T> SourceSheet { get; private set; }
        public Range Range { get; private set; }
        public File File { get; private set; }
        public IEnumerable<int> Keys { get { return _RowOffsets.Keys; } }

        #endregion

        #region Constructors

        public PartialDataSheet(IDataSheet<T> sourceSheet, Range range, File file) {
            SourceSheet = sourceSheet;
            Range = range;
            File = file;

            Build();
        }

        #endregion

        public Language Language { get { return SourceSheet.Language; } }
        public int Count { get { return _RowOffsets.Count; } }

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
                _RowOffsets.Add(key, off);
                //_Rows.Add(key, CreateRow(key, off));
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

        public T this[int key] {
            get {
                T row;
                if (_Rows.TryGetValue(key, out row))
                    return row;

                var offset = _RowOffsets[key];
                _Rows.Add(key, row = CreateRow(key, offset));

                return row;
            }
        }

        #endregion

        #region ISheet Members

        public string Name { get { return SourceSheet.Name + "_" + Range.Start; } }

        public Header Header { get { return SourceSheet.Header; } }

        public ExCollection Collection { get { return SourceSheet.Collection; } }

        public bool ContainsRow(int row) {
            return _RowOffsets.ContainsKey(row);
        }

        IRow ISheet.this[int row] { get { return this[row]; } }

        public object this[int row, int column] { get { return this[row][column]; } }

        #endregion
    }
}
