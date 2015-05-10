using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [System.Diagnostics.DebuggerDisplay("( {X}, {Y}, {Z}, {W} )")]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4 {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public static readonly Vector4 One = new Vector4 { X = 1, Y = 1, Z = 1, W = 1 };
        public static readonly Vector4 Zero = new Vector4 { X = 0, Y = 0, Z = 0, W = 0 };
    }
}
