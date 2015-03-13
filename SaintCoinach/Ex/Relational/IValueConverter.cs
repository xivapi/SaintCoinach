using System;

namespace SaintCoinach.Ex.Relational {
    public interface IValueConverter {
        #region Properties

        string TargetTypeName { get; }
        Type TargetType { get; }

        #endregion

        object Convert(IDataRow row, object rawValue);
    }
}
