using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public class DataRow : IDataRow {
        #region Fields
        private IDataSheet _Sheet;
        private int _Key;
        private int _Offset;
        private Dictionary<int, WeakReference<object>> _ValueReferences = new Dictionary<int, WeakReference<object>>();
        #endregion

        #region Properties
        public IDataSheet Sheet { get { return _Sheet; } }
        ISheet IRow.Sheet { get { return Sheet; } }
        public int Key { get { return _Key; } }
        public int Offset { get { return _Offset; } }
        #endregion

        #region Constructor
        public DataRow(IDataSheet sheet, int key, int offset) {
            const int HeaderLength = 0x06;

            _Sheet = sheet;
            _Key = key;
            _Offset = offset + HeaderLength;
        }
        #endregion

        #region IRow Members

        public object this[int columnIndex] {
            get {
                object value = null;

                if (!_ValueReferences.ContainsKey(columnIndex) || !_ValueReferences[columnIndex].TryGetTarget(out value)) {
                    var column = Sheet.Header.GetColumn(columnIndex);
                    value = column.Read(Sheet.GetBuffer(), this);

                    if (_ValueReferences.ContainsKey(columnIndex))
                        _ValueReferences[columnIndex].SetTarget(value);
                    else
                        _ValueReferences.Add(columnIndex, new WeakReference<object>(value));
                }

                return value;
            }
        }

        #endregion
    }
}
