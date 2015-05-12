using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Imaging {
    // TODO: Come up with a better name for this.
    [StructLayout(LayoutKind.Sequential)]
    public struct GftdEntry {
        public short Id;
        public short X;
        public short Y;
        public short Width;
        public short Height;
        public short Unknown1;
        public short Unknown2;
        public short Unknown3;
    }
}
