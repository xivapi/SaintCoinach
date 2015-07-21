using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    public enum IntegerType {
        Byte = 0xF0,
        ByteTimes256 = 0xF1,
        Int16 = 0xF2,
        Int24 = 0xFA,
        Int32 = 0xFE,
    }
}
