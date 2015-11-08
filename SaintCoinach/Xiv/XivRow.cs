using System;
using System.Linq;
using System.Text;

using SaintCoinach.Ex;
using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Base class for rows inside FFXIV's data sheets.
    /// </summary>
    public class XivRow : IXivRow {
        #region Fields

        /// <summary>
        ///     <see cref="IRelationalRow" /> the current row reads data from.
        /// </summary>
        private readonly IRelationalRow _SourceRow;

        #endregion

        #region Properties

        IRelationalRow IXivRow.SourceRow { get { return _SourceRow; } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="XivRow" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public XivRow(IXivSheet sheet, IRelationalRow sourceRow) {
            Sheet = sheet;
            _SourceRow = sourceRow;
        }

        #endregion

        /// <summary>
        ///     Gets the <see cref="IXivSheet" /> the current row is in.
        /// </summary>
        /// <value>The <see cref="IXivSheet" /> the current row is in.</value>
        public IXivSheet Sheet { get; private set; }

        /// <summary>
        ///     Returns a string representation of the current row.
        /// </summary>
        /// <returns>A string representation of the current row.</returns>
        public override string ToString() {
            return _SourceRow.ToString();
        }

        #region IRelationalRow Members

        IRelationalSheet IRelationalRow.Sheet { get { return Sheet; } }

        public object DefaultValue { get { return _SourceRow.DefaultValue; } }

        public object this[string columnName] { get { return _SourceRow[columnName]; } }

        object IRow.GetRaw(int columnIndex) { return _SourceRow.GetRaw(columnIndex); }
        object IRelationalRow.GetRaw(string columnName) { return _SourceRow.GetRaw(columnName); }

        #endregion

        #region IRow Members

        ISheet IRow.Sheet { get { return Sheet; } }

        public int Key { get { return _SourceRow.Key; } }

        public object this[int columnIndex] { get { return _SourceRow[columnIndex]; } }

        #endregion

        #region Type helpers

        /// <summary>
        ///     Build the full column name from a base and additional indices.
        /// </summary>
        /// <param name="column">Base name of the column.</param>
        /// <param name="indices">Indices for the full name.</param>
        /// <returns>The full column name built using <c>column</c> and <c>indices</c>.</returns>
        public static string BuildColumnName(string column, params int[] indices) {
            var sb = new StringBuilder();
            sb.Append(column);
            foreach (var i in indices) {
                sb.Append('[');
                sb.Append(i);
                sb.Append(']');
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Get the value of the field in the column with the same name as a specific type.
        /// </summary>
        /// <typeparam name="T">The type that should be returned and also the name of the column.</typeparam>
        /// <returns>The value of the field in the column with the same name as the name of type <c>T</c>.</returns>
        public T As<T>() {
            return As<T>(typeof(T).Name);
        }

        /// <summary>
        ///     Get the value of the field in the column with the same base name as a specific type and given indices.
        /// </summary>
        /// <typeparam name="T">The type that should be returned and also the name of the column.</typeparam>
        /// <param name="indices">Indices for the full column.</param>
        /// <returns>The value of the field in the column with the same base name as the name of type <c>T</c> and <c>indices</c>.</returns>
        public T As<T>(params int[] indices) {
            return As<T>(typeof(T).Name, indices);
        }

        /// <summary>
        ///     Get the value of a field in a specific column.
        /// </summary>
        /// <typeparam name="T">The type that should be returned.</typeparam>
        /// <param name="column">Name of the column from which to read.</param>
        /// <returns>The value of the field in <c>column</c>.</returns>
        public T As<T>(string column) {
            return (T)this[column];
        }

        /// <summary>
        ///     Get the value of a field in a specific column and indices.
        /// </summary>
        /// <typeparam name="T">The type that should be returned.</typeparam>
        /// <param name="column">Name of the column from which to read.</param>
        /// <param name="indices">Indices for the full column.</param>
        /// <returns>The value of the field in <c>column</c> at <c>indices</c>.</returns>
        public T As<T>(string column, params int[] indices) {
            return As<T>(BuildColumnName(column, indices));
        }

        /// <summary>
        ///     Gets the value of a field from a specific column as an <see cref="ImageFile" />.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <returns>The <see cref="ImageFile" /> in the <c>column</c> of the current row.</returns>
        public ImageFile AsImage(string column) {
            return (ImageFile)this[column];
        }

        /// <summary>
        ///     Gets the value of a field from a specific column and indices as an <see cref="ImageFile" />.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <param name="indices">Indices for the full column.</param>
        /// <returns>The <see cref="ImageFile" /> in the <c>column</c> at <c>indices</c> of the current row.</returns>
        public ImageFile AsImage(string column, params int[] indices) {
            return AsImage(BuildColumnName(column, indices));
        }

        /// <summary>
        ///     Gets the value of a field from a specific column as a string.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <returns>The string value of the field in <c>column</c> of the current row.</returns>
        public Text.XivString AsString(string column) {
            return (Text.XivString)this[column];
        }

        /// <summary>
        ///     Gets the value of a field from a specific column and indices as a string.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <param name="indices">Indices for the full column.</param>
        /// <returns>The string value of the field in <c>column</c> and <c>indices</c> of the current row.</returns>
        public Text.XivString AsString(string column, params int[] indices) {
            return AsString(BuildColumnName(column, indices));
        }

        /// <summary>
        ///     Gets the value of a field from a specific column as a boolean.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <returns>The boolean value of the field in <c>column</c> of the current row.</returns>
        public Boolean AsBoolean(string column) {
            return Convert.ToBoolean(this[column]);
        }

        /// <summary>
        ///     Gets the value of a field from a specific column and indices as a boolean.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <param name="indices">Indices for the full column.</param>
        /// <returns>The boolean value of the field in <c>column</c> and <c>indices</c> of the current row.</returns>
        public Boolean AsBoolean(string column, params int[] indices) {
            return AsBoolean(BuildColumnName(column, indices));
        }

        /// <summary>
        ///     Gets the value of a field from a specific column as a 16-bit signed integer.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <returns>The 16-bit signed integer value of the field in <c>column</c> of the current row.</returns>
        public Int16 AsInt16(string column) {
            return Convert.ToInt16(this[column]);
        }

        /// <summary>
        ///     Gets the value of a field from a specific column and indices as a 16-bit signed integer.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <param name="indices">Indices for the full column.</param>
        /// <returns>The 16-bit signed integer value of the field in <c>column</c> and <c>indices</c> of the current row.</returns>
        public Int16 AsInt16(string column, params int[] indices) {
            return AsInt16(BuildColumnName(column, indices));
        }

        /// <summary>
        ///     Gets the value of a field from a specific column as a 32-bit signed integer.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <returns>The 32-bit signed integer value of the field in <c>column</c> of the current row.</returns>
        public Int32 AsInt32(string column) {
            return Convert.ToInt32(this[column]);
        }

        /// <summary>
        ///     Gets the value of a field from a specific column and indices as a 32-bit signed integer.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <param name="indices">Indices for the full column.</param>
        /// <returns>The 32-bit signed integer value of the field in <c>column</c> and <c>indices</c> of the current row.</returns>
        public Int32 AsInt32(string column, params int[] indices) {
            return AsInt32(BuildColumnName(column, indices));
        }

        /// <summary>
        ///     Gets the value of a field from a specific column as a 64-bit signed integer.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <returns>The 64-bit signed integer value of the field in <c>column</c> of the current row.</returns>
        public Int64 AsInt64(string column) {
            return Convert.ToInt64(this[column]);
        }

        /// <summary>
        ///     Gets the value of a field from a specific column and indices as a 64-bit signed integer.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <param name="indices">Indices for the full column.</param>
        /// <returns>The 64-bit signed integer value of the field in <c>column</c> and <c>indices</c> of the current row.</returns>
        public Int64 AsInt64(string column, params int[] indices) {
            return AsInt64(BuildColumnName(column, indices));
        }

        /// <summary>
        ///     Gets the value of a field from a specific column as a single-precision floating point value.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <returns>The single-precision floating point value of the field in <c>column</c> of the current row.</returns>
        public Single AsSingle(string column) {
            return Convert.ToSingle(this[column]);
        }

        /// <summary>
        ///     Gets the value of a field from a specific column and indices as a single-precision floating point value.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <param name="indices">Indices for the full column.</param>
        /// <returns>The single-precision floating point value of the field in <c>column</c> and <c>indices</c> of the current row.</returns>
        public Single AsSingle(string column, params int[] indices) {
            return AsSingle(BuildColumnName(column, indices));
        }

        /// <summary>
        ///     Gets the value of a field from a specific column as a double-precision floating point value.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <returns>The double-precision floating point value of the field in <c>column</c> of the current row.</returns>
        public Double AsDouble(string column) {
            return Convert.ToDouble(this[column]);
        }

        /// <summary>
        ///     Gets the value of a field from a specific column and indices as a double-precision floating point value.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <param name="indices">Indices for the full column.</param>
        /// <returns>The double-precision floating point value of the field in <c>column</c> and <c>indices</c> of the current row.</returns>
        public Double AsDouble(string column, params int[] indices) {
            return AsDouble(BuildColumnName(column, indices));
        }

        /// <summary>
        ///     Gets the value of a field from a specific column as a Quad value.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <returns>The quad value of the field in <c>column</c> of the current row.</returns>
        public Quad AsQuad(string column) {
            return (Quad)this[column];
        }

        /// <summary>
        ///     Gets the value of a field from a specific column and indices as a Quad value.
        /// </summary>
        /// <param name="column">Name of the column from which to read.</param>
        /// <param name="indices">Indices for the full column.</param>
        /// <returns>The quad value of the field in <c>column</c> and <c>indices</c> of the current row.</returns>
        public Quad AsQuad(string column, params int[] indices) {
            return AsQuad(BuildColumnName(column, indices));
        }
        #endregion
    }
}
