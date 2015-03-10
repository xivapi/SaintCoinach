using System.Collections.Generic;

namespace SaintCoinach.Ex.Relational {
    public interface IRelationalSheet : ISheet {
        #region Properties

        new RelationalHeader Header { get; }
        new RelationalExCollection Collection { get; }
        new IRelationalRow this[int row] { get; }
        object this[int row, string columnName] { get; }

        #endregion
    }
}
