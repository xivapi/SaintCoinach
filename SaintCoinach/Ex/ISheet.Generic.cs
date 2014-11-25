using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public interface ISheet<T> : IEnumerable<T>, ISheet where T : IRow {
        new IEnumerable<T> GetAllRows();
        new T this[int row] { get; }
    }
}
