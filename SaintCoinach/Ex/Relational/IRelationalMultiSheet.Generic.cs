using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public interface IRelationalMultiSheet<TMulti, TData> : IRelationalSheet, IMultiSheet<TMulti, TData>, IRelationalSheet<TMulti>, IRelationalMultiSheet
        where TMulti : IRelationalMultiRow
        where TData : IRelationalDataRow {
        new IRelationalSheet<TData> ActiveSheet { get; }
        new IRelationalSheet<TData> GetLocalisedSheet(Language language);

        new IEnumerable<TMulti> GetAllRows();
        new TMulti this[int row] { get; }
    }
}
