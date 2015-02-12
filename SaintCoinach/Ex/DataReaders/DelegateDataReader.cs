using System;

namespace SaintCoinach.Ex.DataReaders {
    /// <summary>
    /// Implementation of <see cref="DataReader"/> for reading buffer using a delegate.
    /// </summary>
    public class DelegateDataReader : DataReader {
        /// <summary>
        /// Delegate invoked when reading data.
        /// </summary>
        /// <param name="buffer">A byte-array containing the contents of the EX buffer file.</param>
        /// <param name="offset">The position of the data inside <c>buffer</c>.</param>
        /// <returns>Returns the read value at <c>offset</c> inside <c>buffer</c>.</returns>
        public delegate object ReadDelegate(byte[] buffer, int offset);

        #region Fields

        /// <summary>
        /// The delegate used to read from the buffer file.
        /// </summary>
        private readonly ReadDelegate _Func;
        /// <summary>
        /// The length of the binary buffer in bytes.
        /// </summary>
        private readonly int _Length;
        /// <summary>
        /// The name of the value type read.
        /// </summary>
        private readonly string _Name;
        /// <summary>
        /// The <see cref="Type" /> of the read values.
        /// </summary>
        private readonly Type _Type;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the name of the value type read.
        /// </summary>
        /// <value>The name of the value type read.</value>
        public override string Name { get { return _Name; } }
        /// <summary>
        ///     Gets the length of the binary buffer in bytes.
        /// </summary>
        /// <value>The length of the binary buffer in bytes.</value>
        public override int Length { get { return _Length; } }
        /// <summary>
        ///     Gets the <see cref="Type" /> of the read values.
        /// </summary>
        /// <value>The <see cref="Type" /> of the read values.</value>
        public override Type Type { get { return _Type; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateDataReader"/> class.
        /// </summary>
        /// <param name="name">The name of the value type read.</param>
        /// <param name="length">The length of the binary buffer in bytes.</param>
        /// <param name="type">The <see cref="Type" /> of the read values.</param>
        /// <param name="func">The delegate used to read from the buffer file.</param>
        internal DelegateDataReader(string name, int length, Type type, ReadDelegate func) {
            _Name = name;
            _Length = length;
            _Type = type;
            _Func = func;
        }

        #endregion

        /// <summary>
        ///     Read a column's buffer of a row.
        /// </summary>
        /// <param name="buffer">A byte-array containing the contents of the EX buffer file.</param>
        /// <param name="col"><see cref="Column" /> to read.</param>
        /// <param name="row"><see cref="IDataRow" /> to read in.</param>
        /// <returns>Returns the value read from the given <c>row</c> and <c>column</c>.</returns>
        public override object Read(byte[] buffer, Column col, IDataRow row) {
            var offset = GetFieldOffset(col, row);
            return _Func(buffer, offset);
        }
    }
}
