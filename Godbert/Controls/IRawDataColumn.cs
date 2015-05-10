using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godbert.Controls {
    public interface IRawDataColumn {
        IComparer<object> GetComparer(System.ComponentModel.ListSortDirection direction);
    }
}
