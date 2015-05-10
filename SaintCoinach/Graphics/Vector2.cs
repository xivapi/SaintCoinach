using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [System.Diagnostics.DebuggerDisplay("( {X}, {Y} )")]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2 {
        public float X;
        public float Y;

        public static readonly Vector2 One = new Vector2 { X = 1, Y = 1 };
        public static readonly Vector2 Zero = new Vector2 { X = 0, Y = 0 };
    }
}
