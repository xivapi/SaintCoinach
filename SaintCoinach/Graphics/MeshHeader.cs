using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [StructLayout(LayoutKind.Sequential)]
    public struct MeshHeader {
        public int VertexCount;
        public int IndexCount;
        public short MaterialIndex;
        public short PartOffset;
        public short PartCount;
        public short FileDataPart;
        public int IndexDataOffset;
        public int VertexData1Offset;
        public int VertexData2Offset;
        public short Unknown2;
        public short Unknown3;
        public byte BytesPerVertexData1;
        public byte BytesPerVertexData2;
        public byte Unknown4;
        public byte Unknown5;
    }
}
