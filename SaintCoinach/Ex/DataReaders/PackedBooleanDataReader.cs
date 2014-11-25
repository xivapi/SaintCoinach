using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.DataReaders {
    public class PackedBooleanDataReader : DataReader {
        #region Fields
        private byte _Mask;
        private string _Name;
        #endregion

        #region Properties
        public override int Length {
            get { return 1; }
        }
        public override string Name {
            get { return _Name; }
        }
        public override Type Type {
            get { return typeof(bool); }
        }
        #endregion

        #region Constructor
        public PackedBooleanDataReader(byte mask) {
            _Mask = mask;
            _Name = string.Format("bit&{0:X2}", mask);
        }
        #endregion

        #region Convert
        public override object Read(byte[] data, Column col, IDataRow row) {
            var offset = GetFieldOffset(col, row);
            return (data[offset] & _Mask) != 0;
        }
        #endregion
    }
}
