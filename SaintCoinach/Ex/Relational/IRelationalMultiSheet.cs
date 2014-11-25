using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public interface IRelationalMultiSheet : IRelationalSheet, IMultiSheet {
        new IRelationalSheet ActiveSheet { get; }
        new IRelationalSheet GetLocalisedSheet(Language language);

        new IEnumerable<IRelationalMultiRow> GetAllRows();
        new IRelationalMultiRow this[int row] { get; }
    }
}
