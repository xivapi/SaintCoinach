using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public interface IXivSheet<out T> : IRelationalSheet<T>, IXivSheet where T : IXivRow {
        #region Properties

        new T this[int key] { get; }

        #endregion
    }
}
