namespace SaintCoinach.Ex {
    public interface IMultiRow : IRow {
        #region Properties

        new IMultiSheet Sheet { get; }
        object this[int columnIndex, Language language] { get; }

        object GetRaw(int columnIndex, Language language);

        #endregion
    }
}
