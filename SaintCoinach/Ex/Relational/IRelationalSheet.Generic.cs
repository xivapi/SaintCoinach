using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public interface IRelationalSheet<T> : ISheet<T>, IRelationalSheet where T : IRelationalRow {

    }
}
