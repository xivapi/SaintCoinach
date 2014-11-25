using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.DataReaders {
    public class DelegateDataReader : DataReader {
        #region Fields
        private Func<byte[], int, object> _Func;
        private string _Name;
        private int _Length;
        private Type _Type;
        #endregion

        #region Properties
        public override string Name {
            get { return _Name; }
        }
        public override int Length {
            get { return _Length; }
        }
        public override Type Type {
            get { return _Type; }
        }
        #endregion

        #region Constructor
        internal DelegateDataReader(string name, int length, Type type, Func<byte[], int, object> func) {
            _Name = name;
            _Length = length;
            _Type = type;
            _Func = func;
        }
        #endregion

        public override object Read(byte[] data, Column col, IDataRow row) {
            var offset = GetFieldOffset(col, row);
            return _Func(data, offset);
        }
    }
}
