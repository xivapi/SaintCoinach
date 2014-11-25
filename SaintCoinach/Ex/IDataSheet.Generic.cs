using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public interface IDataSheet<T> : ISheet<T>, IDataSheet
        where T : IDataRow {
    }
}
