using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public interface IMultiSheet : ISheet {
        ISheet ActiveSheet { get; }

        ISheet GetLocalisedSheet(Language language);

        new IEnumerable<IMultiRow> GetAllRows();
        new IMultiRow this[int row] { get; }
    }
}
