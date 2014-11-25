using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public interface IRelationalMultiRow : IRelationalRow, IMultiRow {
        new IRelationalMultiSheet Sheet { get; }

        object this[string columnName, Language language] { get; }
    }
}
