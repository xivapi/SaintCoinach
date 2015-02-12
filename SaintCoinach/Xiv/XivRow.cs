using System;
using System.Linq;

using SaintCoinach.Ex;
using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class XivRow : IXivRow {
        #region Fields

        private readonly IRelationalRow _SourceRow;

        #endregion

        #region Constructors

        #region Constructor

        public XivRow(IXivSheet sheet, IRelationalRow sourceRow) {
            Sheet = sheet;
            _SourceRow = sourceRow;
        }

        #endregion

        #endregion

        public IXivSheet Sheet { get; private set; }

        public override string ToString() {
            return _SourceRow.ToString();
        }

        #region IRelationalRow Members

        IRelationalSheet IRelationalRow.Sheet { get { return Sheet; } }

        public object DefaultValue { get { return _SourceRow.DefaultValue; } }

        public object this[string columnName] { get { return _SourceRow[columnName]; } }

        #endregion

        #region IRow Members

        ISheet IRow.Sheet { get { return Sheet; } }

        public int Key { get { return _SourceRow.Key; } }

        public object this[int columnIndex] { get { return _SourceRow[columnIndex]; } }

        #endregion

        #region Type helpers

        private static string BuildColumnName(string column, params int[] indices) {
            return string.Format("{0}{1}", column,
                string.Join(string.Empty, indices.Select(_ => string.Format("[{0}]", _))));
        }

        public T As<T>() {
            return As<T>(typeof(T).Name);
        }

        public T As<T>(params int[] indices) {
            return As<T>(typeof(T).Name, indices);
        }

        public T As<T>(string column) {
            return (T)this[column];
        }

        public T As<T>(string column, params int[] indices) {
            return As<T>(BuildColumnName(column, indices));
        }

        public ImageFile AsImage(string column) {
            return (ImageFile)this[column];
        }

        public ImageFile AsImage(string column, params int[] indices) {
            return AsImage(BuildColumnName(column, indices));
        }

        public string AsString(string column) {
            return (string)this[column];
        }

        public string AsString(string column, params int[] indices) {
            return AsString(BuildColumnName(column, indices));
        }

        public Boolean AsBoolean(string column) {
            return Convert.ToBoolean(this[column]);
        }

        public Boolean AsBoolean(string column, params int[] indices) {
            return AsBoolean(BuildColumnName(column, indices));
        }

        public Int16 AsInt16(string column) {
            return Convert.ToInt16(this[column]);
        }

        public Int16 AsInt16(string column, params int[] indices) {
            return AsInt16(BuildColumnName(column, indices));
        }

        public Int32 AsInt32(string column) {
            return Convert.ToInt32(this[column]);
        }

        public Int32 AsInt32(string column, params int[] indices) {
            return AsInt32(BuildColumnName(column, indices));
        }

        public Int64 AsInt64(string column) {
            return Convert.ToInt64(this[column]);
        }

        public Int64 AsInt64(string column, params int[] indices) {
            return AsInt64(BuildColumnName(column, indices));
        }

        public Single AsSingle(string column) {
            return Convert.ToSingle(this[column]);
        }

        public Single AsSingle(string column, params int[] indices) {
            return AsSingle(BuildColumnName(column, indices));
        }

        public Double AsDouble(string column) {
            return Convert.ToDouble(this[column]);
        }

        public Double AsDouble(string column, params int[] indices) {
            return AsDouble(BuildColumnName(column, indices));
        }

        #endregion
    }
}
