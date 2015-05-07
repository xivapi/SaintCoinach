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
    }
}
