using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaintCoinach.Ex.Relational;

namespace Godbert.Controls {
    public interface IRawDataColumn {
        IComparer<object> GetComparer(System.ComponentModel.ListSortDirection direction);
        RelationalColumn Column { get; }
    }
}
