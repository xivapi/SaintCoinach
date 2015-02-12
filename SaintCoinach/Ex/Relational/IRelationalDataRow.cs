namespace SaintCoinach.Ex.Relational {
    public interface IRelationalDataRow : IRelationalRow, IDataRow {
        #region Properties

        new IRelationalDataSheet Sheet { get; }

        #endregion
    }
}
