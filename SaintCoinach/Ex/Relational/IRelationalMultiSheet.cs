using System.Collections.Generic;

namespace SaintCoinach.Ex.Relational {
    public interface IRelationalMultiSheet : IRelationalSheet, IMultiSheet {
        #region Properties

        new IRelationalSheet ActiveSheet { get; }
        new IRelationalMultiRow this[int row] { get; }

        #endregion

        new IRelationalSheet GetLocalisedSheet(Language language);
    }
}
