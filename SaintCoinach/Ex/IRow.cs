using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public interface IRow {
        ISheet Sheet { get; }
        int Key { get; }

        object this[int columnIndex] { get; }
    }
}
