using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Variant2 {
    public class DataRow : DataRowBase {
        const int MetadataLength = 0x06;

        private bool _IsRead;
        private Dictionary<int, ColumnValues> _ColumnCache = new Dictionary<int, ColumnValues>();
        private Dictionary<int, SubRow> _SubRows = new Dictionary<int, SubRow>();

        public int Length { get; private set; }
        public int SubRowCount { get; private set; }

        public IEnumerable<int> SubRowKeys {
            get {
                if (!_IsRead) Read();
                return _SubRows.Keys;
            }
        }
        public IEnumerable<SubRow> SubRows {
            get {
                if (!_IsRead) Read();
                return _SubRows.Values;
            }
        }

        public SubRow GetSubRow(int key) {
            if (!_IsRead) Read();
            return _SubRows[key];
        }

        #region Constructors

        public DataRow(IDataSheet sheet, int key, int offset) : base(sheet, key, offset + MetadataLength) {
            var b = sheet.GetBuffer();
            if (b.Length < offset + MetadataLength) throw new IndexOutOfRangeException();

            Length = OrderedBitConverter.ToInt32(b, offset, true);
            SubRowCount = OrderedBitConverter.ToInt16(b, offset + 4, true);
        }

        #endregion

        protected virtual void Read() {
            _ColumnCache.Clear();
            _SubRows.Clear();

            var h = Sheet.Header;
            foreach (var c in h.Columns)
                _ColumnCache.Add(c.Index, new ColumnValues(c));

            var b = Sheet.GetBuffer();
            var o = Offset;
            for(var i = 0; i < SubRowCount; ++i) {
                var key = OrderedBitConverter.ToInt16(b, o, true);
                o += 2;

                var r = new SubRow(this, key, o);
                _SubRows.Add(key, r);
                
                foreach (var c in _ColumnCache.Values)
                {
                    c.Values.Add(key, r[c.Column.Index]);
                    c.RawValues.Add(key, r.GetRaw(c.Column.Index));
                }

                o += h.FixedSizeDataLength;
            }

            _IsRead = true;
        }
        
        public override object this[int column] {
            get {
                if (!_IsRead) Read();
                return _ColumnCache[column].Values;
            }
        }

        public override object GetRaw(int column) {
            if (!_IsRead) Read();
            return _ColumnCache[column].RawValues;
        }
    }
}
