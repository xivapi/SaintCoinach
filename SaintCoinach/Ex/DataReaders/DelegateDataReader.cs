using System;

namespace SaintCoinach.Ex.DataReaders {
    public class DelegateDataReader : DataReader {
        #region Fields

        private readonly Func<byte[], int, object> _Func;
        private readonly int _Length;
        private readonly string _Name;
        private readonly Type _Type;

        #endregion

        #region Properties

        public override string Name { get { return _Name; } }
        public override int Length { get { return _Length; } }
        public override Type Type { get { return _Type; } }

        #endregion

        #region Constructors

        #region Constructor

        internal DelegateDataReader(string name, int length, Type type, Func<byte[], int, object> func) {
            _Name = name;
            _Length = length;
            _Type = type;
            _Func = func;
        }

        #endregion

        #endregion

        public override object Read(byte[] data, Column col, IDataRow row) {
            var offset = GetFieldOffset(col, row);
            return _Func(data, offset);
        }
    }
}
