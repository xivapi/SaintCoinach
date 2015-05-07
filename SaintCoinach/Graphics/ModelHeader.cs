using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [StructLayout(LayoutKind.Sequential)]
    public struct ModelHeader {
        public short MeshOffset;
        public short MeshCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x14)]
        public ushort[] Unknown1;
        public int VertexDataSize;
        public int IndexDataSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x04)]
        public ushort[] Unknown2;
    }
}
