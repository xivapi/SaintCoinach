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
        public short BoneListIndex;
        public int IndexBufferOffset;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] VertexOffsets;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] BytesPerVertexPerBuffer;
        public byte VertexBufferCount;
    }
}
