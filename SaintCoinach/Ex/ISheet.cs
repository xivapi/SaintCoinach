using System.Collections;
using System.Collections.Generic;

namespace SaintCoinach.Ex {
    public interface ISheet : IEnumerable {
        #region Properties

        string Name { get; }
        Header Header { get; }
        ExCollection Collection { get; }
        int Count { get; }
        IRow this[int row] { get; }
        object this[int row, int column] { get; }
        IEnumerable<int> Keys { get; }

        #endregion

        bool ContainsRow(int row);
    }
}
