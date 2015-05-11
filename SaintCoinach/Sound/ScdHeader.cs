using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Sound {
    [StructLayout(LayoutKind.Sequential)]
    public struct ScdHeader {
        public short Unknown1Count;
        public short Unknown2Count;
        public short EntryCount;
        public short Unknown1;
        public int Unknown1Offset;
        public int EntryTableOffset;
        public int Unknown2Offset;
        public int Unknown2;
        public int UnknownOffset1;
    }
}
