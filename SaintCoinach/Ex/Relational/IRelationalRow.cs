using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public interface IRelationalRow : IRow {
        new IRelationalSheet Sheet { get; }
        object DefaultValue { get; }
        object this[string columnName] { get; }
    }
}
