namespace SaintCoinach.Ex.Relational {
    public interface IRelationalRow : IRow {
        #region Properties

        new IRelationalSheet Sheet { get; }
        object DefaultValue { get; }
        object this[string columnName] { get; }

        object GetRaw(string columnName);

        #endregion
    }
}
