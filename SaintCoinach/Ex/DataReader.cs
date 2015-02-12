using System;
using System.Collections.Generic;

using SaintCoinach.Ex.DataReaders;

namespace SaintCoinach.Ex {
    public abstract class DataReader {
        #region Static

        private static readonly Dictionary<int, DataReader> DataReaders;

        #endregion

        #region Properties

        public abstract string Name { get; }
        public abstract int Length { get; }
        public abstract Type Type { get; }

        #endregion

        #region Constructors

        static DataReader() {
            DataReaders = new Dictionary<int, DataReader> {
                {
                    0x0000, new StringDataReader()
                }, {
                    0x0001, new DelegateDataReader("bool", 1, typeof(bool), (d, o) => d[o] != 0)
                }, {
                    0x0002, new DelegateDataReader("sbyte", 1, typeof(sbyte), (d, o) => (sbyte)d[o])
                }, {
                    0x0003, new DelegateDataReader("byte", 1, typeof(byte), (d, o) => d[o])
                }, {
                    0x0004,
                    new DelegateDataReader("int16", 2, typeof(short), (d, o) => OrderedBitConverter.ToInt16(d, o, true))
                }, {
                    0x0005,
                    new DelegateDataReader("uint16", 2, typeof(ushort),
                        (d, o) => OrderedBitConverter.ToUInt16(d, o, true))
                }, {
                    0x0006,
                    new DelegateDataReader("int32", 4, typeof(int), (d, o) => OrderedBitConverter.ToInt32(d, o, true))
                }, {
                    0x0007,
                    new DelegateDataReader("uint32", 4, typeof(uint), (d, o) => OrderedBitConverter.ToUInt32(d, o, true))
                }, {
                    0x0009,
                    new DelegateDataReader("single", 4, typeof(Single),
                        (d, o) => OrderedBitConverter.ToSingle(d, o, true))
                }, {
                    0x000B,
                    new DelegateDataReader("int64", 8, typeof(Double), (d, o) => OrderedBitConverter.ToInt64(d, o, true))
                }
            };


            for (byte i = 0; i < 8; ++i)
                DataReaders.Add(0x19 + i, new PackedBooleanDataReader((byte)(1 << i)));
        }

        #endregion

        public static DataReader GetReader(int type) {
            DataReader reader;
            if (!DataReaders.TryGetValue(type, out reader))
                throw new NotSupportedException(string.Format("Unsupported data type {0:X4}h", type));
            return reader;
        }

        public abstract object Read(byte[] buffer, Column col, IDataRow row);

        protected static int GetFieldOffset(Column col, IDataRow row) {
            return col.Offset + row.Offset;
        }
    }
}
