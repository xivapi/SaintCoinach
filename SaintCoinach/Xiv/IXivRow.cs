using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public interface IXivRow : Ex.Relational.IRelationalRow {
        new IXivSheet Sheet { get; }
    }
}
