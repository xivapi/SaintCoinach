using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public interface IRelationalDataSheet<T> : IRelationalDataSheet, IDataSheet<T>, IRelationalSheet<T>
        where T : IRelationalDataRow {
        new T this[int key] { get; }
    }
}
