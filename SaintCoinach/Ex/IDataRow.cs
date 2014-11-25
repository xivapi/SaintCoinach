using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public interface IDataRow : IRow {
        int Offset { get; }
        new IDataSheet Sheet { get; }
    }
}
