using System.Collections.Generic;

namespace SaintCoinach.Ex {
    public interface IMultiSheet<out TMulti, out TData> : ISheet<TMulti>, IMultiSheet
        where TMulti : IMultiRow
        where TData : IRow {
        #region Properties

        new ISheet<TData> ActiveSheet { get; }
        new TMulti this[int row] { get; }

        #endregion

        new ISheet<TData> GetLocalisedSheet(Language language);
    }
}
