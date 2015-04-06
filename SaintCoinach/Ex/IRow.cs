namespace SaintCoinach.Ex {
    public interface IRow {
        #region Properties

        ISheet Sheet { get; }
        int Key { get; }
        object this[int columnIndex] { get; }

        object GetRaw(int columnIndex);

        #endregion
    }
}
