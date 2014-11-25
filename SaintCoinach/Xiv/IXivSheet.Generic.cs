using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public interface IXivSheet<T> : Ex.Relational.IRelationalSheet<T>, IXivSheet where T : IXivRow {
        new T this[int key] { get; }
    }
}
