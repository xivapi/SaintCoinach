using System.Collections.Generic;

namespace SaintCoinach.Ex.Relational {
    public interface IRelationalMultiSheet<out TMulti, out TData> : IMultiSheet<TMulti, TData>,
                                                                    IRelationalSheet<TMulti>, IRelationalMultiSheet
        where TMulti : IRelationalMultiRow
        where TData : IRelationalDataRow {
        #region Properties

        new IRelationalSheet<TData> ActiveSheet { get; }
        new TMulti this[int row] { get; }

        #endregion

        new IRelationalSheet<TData> GetLocalisedSheet(Language language);
    }
}
