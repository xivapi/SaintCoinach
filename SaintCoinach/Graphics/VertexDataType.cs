using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public enum VertexDataType : byte {
        Single3 = 0x02,
        Single4 = 0x03,
        UInt = 0x05,
        ByteFloat4 = 0x08,
        Half2 = 0x0D,
        Half4 = 0x0E,
    }
}
