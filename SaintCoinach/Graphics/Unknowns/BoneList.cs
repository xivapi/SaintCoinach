using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Unknowns {
    [StructLayout(LayoutKind.Sequential)]
    public struct BoneList {  // 3 in hsl, something to do with bones (hierarchy? though it looks nothing like a hierarchy)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x40)]
        public ushort[] Bones;

        public uint ActualCount;
    }
}
