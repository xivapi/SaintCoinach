namespace SaintCoinach.Ex {
    public interface IDataRow : IRow {
        #region Properties

        int Offset { get; }
        new IDataSheet Sheet { get; }

        #endregion
    }
}
