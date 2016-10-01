using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Variant1 {
    public class DataRow : DataRowBase {
        const int MetadataLength = 0x06;

        #region Fields
        
        private readonly Dictionary<int, WeakReference<object>> _ValueReferences =
            new Dictionary<int, WeakReference<object>>();

        #endregion

        public int Length { get; private set; }

        #region Constructors

        public DataRow(IDataSheet sheet, int key, int offset) : base(sheet, key, offset + MetadataLength) {
            var b = sheet.GetBuffer();
            if (b.Length < offset + MetadataLength) throw new IndexOutOfRangeException();

            Length = OrderedBitConverter.ToInt32(b, offset, true);
            var c = OrderedBitConverter.ToInt16(b, offset + 4, true);
            if (c != 1) throw new InvalidDataException();
        }

        #endregion

        #region IRow Members

        public override object this[int columnIndex] {
            get {
                object value;

                if (_ValueReferences.ContainsKey(columnIndex) && _ValueReferences[columnIndex].TryGetTarget(out value))
                    return value;

                var column = Sheet.Header.GetColumn(columnIndex);
                value = column.Read(Sheet.GetBuffer(), this);

                if (_ValueReferences.ContainsKey(columnIndex))
                    _ValueReferences[columnIndex].SetTarget(value);
                else
                    _ValueReferences.Add(columnIndex, new WeakReference<object>(value));

                return value;
            }
        }

        public override object GetRaw(int columnIndex) {
            var column = Sheet.Header.GetColumn(columnIndex);
            return column.ReadRaw(Sheet.GetBuffer(), this);
        }
        
        #endregion
    }
}
