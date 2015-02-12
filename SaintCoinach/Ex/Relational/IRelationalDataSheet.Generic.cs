namespace SaintCoinach.Ex.Relational {
    public interface IRelationalDataSheet<out T> : IRelationalDataSheet, IDataSheet<T>, IRelationalSheet<T>
        where T : IRelationalDataRow {
        #region Properties

        new T this[int key] { get; }

        #endregion
    }
}
