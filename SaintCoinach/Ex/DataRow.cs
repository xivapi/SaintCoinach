using System;
using System.Collections.Generic;

namespace SaintCoinach.Ex {
    public class DataRow : IDataRow {
        #region Fields

        private readonly Dictionary<int, WeakReference<object>> _ValueReferences =
            new Dictionary<int, WeakReference<object>>();

        #endregion

        #region Constructors

        #region Constructor

        public DataRow(IDataSheet sheet, int key, int offset) {
            const int HeaderLength = 0x06;

            Sheet = sheet;
            Key = key;
            Offset = offset + HeaderLength;
        }

        #endregion

        #endregion

        public IDataSheet Sheet { get; private set; }
        ISheet IRow.Sheet { get { return Sheet; } }
        public int Key { get; private set; }
        public int Offset { get; private set; }

        #region IRow Members

        public object this[int columnIndex] {
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

        object IRow.GetRaw(int columnIndex) {
            var column = Sheet.Header.GetColumn(columnIndex);
            return column.ReadRaw(Sheet.GetBuffer(), this);
        }

        #endregion
    }
}
