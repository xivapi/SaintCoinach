using System;
using System.Collections.Generic;

namespace SaintCoinach.Ex {
    public abstract class DataRowBase : IDataRow {

        #region Constructors

        protected DataRowBase(IDataSheet sheet, int key, int offset) {
            Sheet = sheet;
            Key = key;
            Offset = offset;
        }

        #endregion

        public IDataSheet Sheet { get; private set; }
        ISheet IRow.Sheet { get { return Sheet; } }
        public int Key { get; private set; }
        public int Offset { get; private set; }

        #region IRow Members

        public abstract object this[int columnIndex] { get; }
        public abstract object GetRaw(int columnIndex);

        #endregion
    }
}
