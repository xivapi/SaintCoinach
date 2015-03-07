using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public interface IXivSheet : IRelationalSheet {
        #region Properties

        new XivCollection Collection { get; }
        new IXivRow this[int row] { get; }

        #endregion
    }
}
