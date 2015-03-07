using System.Collections.Generic;

namespace SaintCoinach.Ex {
    public interface IMultiSheet : ISheet {
        #region Properties

        ISheet ActiveSheet { get; }
        new IMultiRow this[int row] { get; }

        #endregion

        ISheet GetLocalisedSheet(Language language);
    }
}
