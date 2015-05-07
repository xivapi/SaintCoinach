using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [StructLayout(LayoutKind.Sequential)]
    public struct ImcVariant {
        public short Variant;
        public ushort PartVisibilityMask;
        public byte Unknown3;
        public byte Unknown4;

        public static readonly ImcVariant Default = new ImcVariant {
            Variant = 1,
            PartVisibilityMask = 0x03FF,
            Unknown3 = 0x00,
            Unknown4 = 0x00
        };
    }
}
