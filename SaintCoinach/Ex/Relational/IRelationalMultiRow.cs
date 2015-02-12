namespace SaintCoinach.Ex.Relational {
    public interface IRelationalMultiRow : IRelationalRow, IMultiRow {
        #region Properties

        new IRelationalMultiSheet Sheet { get; }
        object this[string columnName, Language language] { get; }

        #endregion
    }
}
