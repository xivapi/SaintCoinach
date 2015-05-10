using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Unknowns {
    [StructLayout(LayoutKind.Sequential)]
    public struct MaterialStruct2 {
        public uint Unknown1;   // Probably an Id of some sort
        public ushort Offset;   // Offset & Size are relative to the blob of data at the end, after ParameterMappings
        public ushort Size;
    }
}
