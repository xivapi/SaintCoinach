using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [System.Diagnostics.DebuggerDisplay("( {X}, {Y}, {Z} )")]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3 {
        public float X;
        public float Y;
        public float Z;

        public static readonly Vector3 One = new Vector3 { X = 1, Y = 1, Z = 1 };
        public static readonly Vector3 Zero = new Vector3 { X = 0, Y = 0, Z = 0 };
    }
}
