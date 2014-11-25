using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update {
    [Flags]
    public enum ChangeType {
        None,
        Structure,
        Data,
        Breaking
    }
}
