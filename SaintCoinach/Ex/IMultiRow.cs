namespace SaintCoinach.Ex {
    public interface IMultiRow : IRow {
        #region Properties

        new IMultiSheet Sheet { get; }
        object this[int columnIndex, Language language] { get; }

        #endregion
    }
}
