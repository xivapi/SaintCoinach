using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public abstract partial class DataReader {
        #region Static
        private static Dictionary<int, DataReader> _DataReaders;

        static DataReader() {
            _DataReaders = new Dictionary<int, DataReader>();
            
            _DataReaders.Add(0x0000, new DataReaders.StringDataReader());
            _DataReaders.Add(0x0001, new DataReaders.DelegateDataReader("bool", 1, typeof(bool), (d, o) => d[o] != 0));
            _DataReaders.Add(0x0002, new DataReaders.DelegateDataReader("sbyte", 1, typeof(sbyte), (d, o) => (sbyte)d[o]));
            _DataReaders.Add(0x0003, new DataReaders.DelegateDataReader("byte", 1, typeof(byte), (d, o) => d[o]));
            _DataReaders.Add(0x0004, new DataReaders.DelegateDataReader("int16", 2, typeof(short), (d, o) => OrderedBitConverter.ToInt16(d, o, true)));
            _DataReaders.Add(0x0005, new DataReaders.DelegateDataReader("uint16", 2, typeof(ushort), (d, o) => OrderedBitConverter.ToUInt16(d, o, true)));
            _DataReaders.Add(0x0006, new DataReaders.DelegateDataReader("int32", 4, typeof(int), (d, o) => OrderedBitConverter.ToInt32(d, o, true)));
            _DataReaders.Add(0x0007, new DataReaders.DelegateDataReader("uint32", 4, typeof(uint), (d, o) => OrderedBitConverter.ToUInt32(d, o, true)));

            _DataReaders.Add(0x0009, new DataReaders.DelegateDataReader("single", 4, typeof(Single), (d, o) => OrderedBitConverter.ToSingle(d, o, true)));

            _DataReaders.Add(0x000B, new DataReaders.DelegateDataReader("int64", 8, typeof(Double), (d, o) => OrderedBitConverter.ToInt64(d, o, true)));

            for (byte i = 0; i < 8; ++i)
                _DataReaders.Add(0x19 + i, new DataReaders.PackedBooleanDataReader((byte)(1 << i)));
        }

        public static DataReader GetReader(int type) {
            DataReader reader;
            if (!_DataReaders.TryGetValue(type, out reader))
                throw new NotSupportedException(string.Format("Unsupported data type {0:X4}h", type));
            return reader;
        }
        #endregion

        public abstract string Name { get; }
        public abstract int Length { get; }
        public abstract Type Type { get; }

        public abstract object Read(byte[] buffer, Column col, IDataRow row);

        protected static int GetFieldOffset(Column col, IDataRow row) {
            return col.Offset + row.Offset;
        }
    }
}
