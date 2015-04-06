namespace SaintCoinach.Ex {
    /// <summary>
    ///     Class for represeting columns inside EX files.
    /// </summary>
    public class Column {
        #region Properties

        /// <summary>
        ///     Gets the <see cref="Header" /> of the EX file the column is in.
        /// </summary>
        /// <value>The <see cref="Header" /> of the EX file the column is in.</value>
        public Header Header { get; private set; }

        /// <summary>
        ///     Gets the index of the column inside the EX file.
        /// </summary>
        /// <value>The index of the column inside the EX file.</value>
        public int Index { get; private set; }

        /// <summary>
        ///     Gets the integer identifier for the type of the column's data.
        /// </summary>
        /// <remarks>
        ///     This value is read from the source header to get the correct object for <see cref="Reader" />, should not be
        ///     required any further.
        /// </remarks>
        /// <value>The integer identifier for the type of the column's data.</value>
        public int Type { get; private set; }

        /// <summary>
        ///     Gets the position of the column's data in a row.
        /// </summary>
        /// <value>The position of the column's data in a row.</value>
        public int Offset { get; private set; }

        /// <summary>
        ///     Gets the <see cref="DataReader" /> used to read column's data.
        /// </summary>
        /// <value>The <see cref="DataReader" /> used to read column's data.</value>
        public DataReader Reader { get; private set; }

        /// <summary>
        ///     Gets a string indicating what type the column's contents are.
        /// </summary>
        /// <value>A string indicating what type the column's contents are.</value>
        public virtual string ValueType { get { return Reader.Name; } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Column" /> class.
        /// </summary>
        /// <param name="header">The <see cref="Header" /> of the EX file the column is in.</param>
        /// <param name="index">The index of the column inside the EX file.</param>
        /// <param name="buffer">A byte-array containing the contents of the header file.</param>
        /// <param name="offset">The position of the column information inside <c>buffer</c>.</param>
        public Column(Header header, int index, byte[] buffer, int offset) {
            const int TypeOffset = 0x00;
            const int PositionOffset = 0x02;

            Header = header;
            Index = index;
            Type = OrderedBitConverter.ToUInt16(buffer, offset + TypeOffset, true);
            Offset = OrderedBitConverter.ToUInt16(buffer, offset + PositionOffset, true);

            Reader = DataReader.GetReader(Type);
        }

        #endregion

        #region Read

        /// <summary>
        ///     Read the column's value in a row, possibly post-processed depending on the column's implementation.
        /// </summary>
        /// <param name="buffer">A byte-array containing the contents of the data file.</param>
        /// <param name="row">The <see cref="IDataRow" /> whose data should be read.</param>
        /// <returns>Returns the column's value in <c>row</c>.</returns>
        public virtual object Read(byte[] buffer, IDataRow row) {
            return ReadRaw(buffer, row);
        }

        /// <summary>
        ///     Read the raw column's value in a row.
        /// </summary>
        /// <param name="buffer">A byte-array containing the contents of the data file.</param>
        /// <param name="row">The <see cref="IDataRow" /> whose data should be read.</param>
        /// <returns>Returns the raw column's value in <c>row</c>.</returns>
        public object ReadRaw(byte[] buffer, IDataRow row) {
            return Reader.Read(buffer, this, row);
        }
        #endregion
    }
}
