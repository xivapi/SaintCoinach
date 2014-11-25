using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public interface IRelationalSheet : ISheet {
        new RelationalHeader Header { get; }
        new RelationalExCollection Collection { get; }
        new IEnumerable<IRelationalRow> GetAllRows();
        new IRelationalRow this[int row] { get; }
        object this[int row, string columnName] { get; }
    }
}
