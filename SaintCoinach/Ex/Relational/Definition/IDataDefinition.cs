using System;

namespace SaintCoinach.Ex.Relational.Definition {
    public interface IDataDefinition {
        #region Properties

        int Length { get; }

        #endregion

        /// <param name="row">The row to convert a value of.</param>
        /// <param name="value">Raw value as read from the file.</param>
        /// <param name="index">Index in the definition on which the method is called.</param>
        object Convert(IDataRow row, object value, int index);

        /// <param name="index">Index in the definition on which the method is called.</param>
        string GetName(int index);

        /// <param name="index">Index in the definition on which the method is called.</param>
        string GetValueTypeName(int index);

        /// <param name="index">Index in the definition on which the method is called.</param>
        Type GetValueType(int index);

        IDataDefinition Clone();
    }
}
