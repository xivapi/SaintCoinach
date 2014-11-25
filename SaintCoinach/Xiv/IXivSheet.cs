using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public interface IXivSheet : Ex.Relational.IRelationalSheet {
        new XivCollection Collection { get; }
        new IEnumerable<IXivRow> GetAllRows();
        new IXivRow this[int row] { get; }
    }
}
