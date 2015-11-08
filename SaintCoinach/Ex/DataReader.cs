using System;
using System.Collections.Generic;

using SaintCoinach.Ex.DataReaders;
using SaintCoinach.Xiv;

namespace SaintCoinach.Ex {
    /// <summary>
    ///     Base class used in reading data from EX data files.
    /// </summary>
    public abstract class DataReader {
        #region Static

        /// <summary>
        ///     Mappings of type identifiers used in EX headers to their corresponding <see cref="DataReader" />.
        /// </summary>
        private static readonly Dictionary<int, DataReader> DataReaders;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the name of the value type read.
        /// </summary>
        /// <value>The name of the value type read.</value>
        public abstract string Name { get; }

        /// <summary>
        ///     Gets the length of the binary data in bytes.
        /// </summary>
        /// <remarks>
        ///     Should only return the length for data inside the fixed-data part of a row, not variable-length data present after.
        /// </remarks>
        /// <value>The length of the binary data in bytes.</value>
        public abstract int Length { get; }

        /// <summary>
        ///     Gets the <see cref="Type" /> of the read values.
        /// </summary>
        /// <value>The <see cref="Type" /> of the read values.</value>
        public abstract Type Type { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="DataReader"/> class.
        /// </summary>
        /// <remarks>
        /// This creates the mappings of type identifiers and <see cref="DataReader"/>s.
        /// </remarks>
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
                    new DelegateDataReader("int64", 8, typeof(Quad), (d, o) => Quad.Read(d, o, true))
                }
            };


            for (byte i = 0; i < 8; ++i)
                DataReaders.Add(0x19 + i, new PackedBooleanDataReader((byte)(1 << i)));
        }

        #endregion

        /// <summary>
        ///     Get the <see cref="DataReader" /> for a integer identifier.
        /// </summary>
        /// <param name="type">The integer identifier of the data type.</param>
        /// <returns>Returns the <see cref="DataReader" /> assigned to the given <c>type</c>.</returns>
        /// <exception cref="NotSupportedException"><c>type</c> is not recognized.</exception>
        public static DataReader GetReader(int type) {
            DataReader reader;
            if (!DataReaders.TryGetValue(type, out reader))
                throw new NotSupportedException(string.Format("Unsupported data type {0:X4}h", type));
            return reader;
        }

        /// <summary>
        ///     Read a column's data of a row.
        /// </summary>
        /// <param name="buffer">A byte-array containing the contents of the EX data file.</param>
        /// <param name="col"><see cref="Column" /> to read.</param>
        /// <param name="row"><see cref="IDataRow" /> to read in.</param>
        /// <returns>Returns the value read from the given <c>row</c> and <c>column</c>.</returns>
        public abstract object Read(byte[] buffer, Column col, IDataRow row);

        /// <summary>
        ///     Get the absolute offset in the EX data file of a column for a given row.
        /// </summary>
        /// <param name="col"><see cref="Column" /> whose offset should be returned.</param>
        /// <param name="row"><see cref="IDataRow" /> to use as base.</param>
        /// <returns>Returns the absolute offset in the EX data file <c>col</c> inside <c>row</c>.</returns>
        protected static int GetFieldOffset(Column col, IDataRow row) {
            return col.Offset + row.Offset;
        }
    }
}
