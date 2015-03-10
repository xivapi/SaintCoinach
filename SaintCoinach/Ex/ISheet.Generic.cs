using System.Collections.Generic;

namespace SaintCoinach.Ex {
    public interface ISheet<out T> : IEnumerable<T>, ISheet where T : IRow {
        #region Properties

        new T this[int row] { get; }

        #endregion
    }
}
