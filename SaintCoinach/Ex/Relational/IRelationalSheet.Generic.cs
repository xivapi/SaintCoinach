namespace SaintCoinach.Ex.Relational {
    public interface IRelationalSheet<out T> : ISheet<T>, IRelationalSheet where T : IRelationalRow {
        #region Properties

        new T this[int key] { get; }

        #endregion
    }
}
