using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update {
    [Flags]
    public enum ChangeType {
        None        = 0x00,
        Structure   = 0x01,
        Data        = 0x02,
        Breaking    = 0x04,
    }
}
