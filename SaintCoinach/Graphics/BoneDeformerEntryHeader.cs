using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [StructLayout(LayoutKind.Sequential)]
    public struct BoneDeformerEntryHeader {
        public ushort CharacterType;
        public ushort Key;
        public int Offset;
    }
}
