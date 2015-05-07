using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public struct MaterialHeader {
        public byte Unknown1;
        public byte Unknown2;
        public byte Unknown3;
        public byte Unknown4;
        public ushort FileSize;
        public ushort UnknownSize1;
        public ushort StringsSize;
        public ushort ShaderOffset;
        public byte TextureCount;
        public byte MapCount;
        public byte DataSetCount;
        public byte UnknownSize2;
    }
}
