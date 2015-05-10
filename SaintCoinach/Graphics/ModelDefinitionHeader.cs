using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [StructLayout(LayoutKind.Sequential)]
    public struct ModelDefinitionHeader {
        public uint Unknown1;
        public ushort MeshCount;
        public ushort AttributeCount;
        public ushort PartCount;
        public ushort MaterialCount;
        public ushort BoneCount;
        public ushort UnknownStruct4Count;  // 3 in hsl
        public ushort UnknownStruct5Count;  // 4 in hsl
        public ushort UnknownStruct6Count;  // 5 in hsl
        public ushort UnknownStruct7Count;  // 6 in hsl
        public ushort Unknown2;
        public ushort UnknownStruct1Count;  // 0 in hsl
        public byte UnknownStruct2Count;  // 1 in hsl
        public byte Unknown3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public ushort[] Unknown4;
        public ushort UnknownStruct3Count;  // 7 in hsl
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public ushort[] Unknown5;
    }
}
