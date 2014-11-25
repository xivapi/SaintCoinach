using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public interface ISheet : IEnumerable {
        string Name { get; }
        Header Header { get; }
        ExCollection Collection { get; }

        bool ContainsRow(int row);
        IEnumerable<IRow> GetAllRows();

        IRow this[int row] { get; }
        object this[int row, int column] { get; }
    }
}
