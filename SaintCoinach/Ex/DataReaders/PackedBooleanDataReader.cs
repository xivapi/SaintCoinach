using System;

namespace SaintCoinach.Ex.DataReaders {
    /// <summary>
    /// Implementation of <see cref="DataReader"/> for reading a specific bit inside a byte.
    /// </summary>
    public class PackedBooleanDataReader : DataReader {
        #region Fields

        /// <summary>
        /// Mask used to determine the resulting value.
        /// </summary>
        private readonly byte _Mask;
        /// <summary>
        /// The name of the value type read.
        /// </summary>
        private readonly string _Name;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the length of the binary buffer in bytes.
        /// </summary>
        /// <value><c>1</c></value>
        public override int Length { get { return 1; } }
        /// <summary>
        ///     Gets the name of the value type read.
        /// </summary>
        /// <value>The name of the value type read.</value>
        public override string Name { get { return _Name; } }
        /// <summary>
        ///     Gets the <see cref="Type" /> of the read values.
        /// </summary>
        /// <value>Value is always typeof(<see cref="bool"/>).</value>
        public override Type Type { get { return typeof(bool); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PackedBooleanDataReader"/> class.
        /// </summary>
        /// <param name="mask">Mask to match the read byte against.</param>
        public PackedBooleanDataReader(byte mask) {
            _Mask = mask;
            _Name = string.Format("bit&{0:X2}", mask);
        }

        #endregion

        #region Convert

        /// <summary>
        ///     Read a column's buffer of a row.
        /// </summary>
        /// <param name="buffer">A byte-array containing the contents of the EX buffer file.</param>
        /// <param name="col"><see cref="Column" /> to read.</param>
        /// <param name="row"><see cref="IDataRow" /> to read in.</param>
        /// <returns>Returns the value read from the given <c>row</c> and <c>column</c>.</returns>
        public override object Read(byte[] buffer, Column col, IDataRow row) {
            var offset = GetFieldOffset(col, row);
            return (buffer[offset] & _Mask) != 0;
        }

        #endregion
    }
}
