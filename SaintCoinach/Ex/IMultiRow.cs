using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public interface IMultiRow : IRow {
        new IMultiSheet Sheet { get; }

        object this[int columnIndex, Language language] { get; }
    }
}
