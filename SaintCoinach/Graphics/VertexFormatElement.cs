using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexFormatElement {
        public byte SourcePart;         // 0 = VertexData1 ; 1 = VertexData2 ; 255 = STAHP
        public byte Offset;
        public VertexDataType DataType;
        public VertexAttribute Attribute;
        public int Unknown;             // Always 0 so far
    }
}
