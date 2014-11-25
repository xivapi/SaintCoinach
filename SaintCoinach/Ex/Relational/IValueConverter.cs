using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public interface IValueConverter {
        string TargetTypeName { get; }
        object Convert(IDataRow row, object rawValue);
    }
}
