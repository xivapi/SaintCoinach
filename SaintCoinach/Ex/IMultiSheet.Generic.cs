using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public interface IMultiSheet<TMulti, TData> : ISheet<TMulti>, IMultiSheet 
        where TMulti : IMultiRow 
        where TData : IRow {
        new ISheet<TData> ActiveSheet { get; }
        new ISheet<TData> GetLocalisedSheet(Language language);

        new IEnumerable<TMulti> GetAllRows();
        new TMulti this[int row] { get; }
    }
}
