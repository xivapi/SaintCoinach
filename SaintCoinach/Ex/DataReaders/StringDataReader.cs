using System;

namespace SaintCoinach.Ex.DataReaders {
    /// <summary>
    /// Implementation of <see cref="DataReader"/> for reading strings.
    /// </summary>
    public class StringDataReader : DataReader {
        #region Properties

        /// <summary>
        ///     Gets the length of the binary buffer in bytes.
        /// </summary>
        /// <remarks>
        /// This is only the length of the integer used to store the position of the string. The actual strings are variable-length and appended at the end of a row.
        /// </remarks>
        /// <value><c>4</c></value>
        public override int Length { get { return 4; } }
        /// <summary>
        ///     Gets the name of the value type read.
        /// </summary>
        /// <value>Value is always <c>str</c></value>
        public override string Name { get { return "str"; } }
        /// <summary>
        ///     Gets the <see cref="Type" /> of the read values.
        /// </summary>
        /// <value>Value is always typeof(<see cref="string"/>).</value>
        public override Type Type { get { return typeof(Text.XivString); } }

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
            var fieldOffset = GetFieldOffset(col, row);
            var endOfFixed = row.Offset + row.Sheet.Header.FixedSizeDataLength;

            var start = endOfFixed + OrderedBitConverter.ToInt32(buffer, fieldOffset, true);
            if (start < 0)
                return null;

            var end = start - 1;
            while (++end < buffer.Length && buffer[end] != 0) { }
            var len = end - start;

            if (len == 0)
                return Text.XivString.Empty;

            var binaryString = new byte[len];
            Array.Copy(buffer, start, binaryString, 0, len);

            return Text.XivStringDecoder.Default.Decode(binaryString);//.ToString();
        }

        #endregion
    }
}
