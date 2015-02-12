namespace SaintCoinach.Ex.Relational {
    public interface IValueConverter {
        #region Properties

        string TargetTypeName { get; }

        #endregion

        object Convert(IDataRow row, object rawValue);
    }
}
