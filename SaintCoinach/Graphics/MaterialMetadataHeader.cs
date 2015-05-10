using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [StructLayout(LayoutKind.Sequential)]
    public struct MaterialMetadataHeader {
        public ushort DataSize;     // Bytes at the end
        public ushort UnknownStruct1Count;
        public ushort UnknownStruct2Count;
        public ushort ParameterMappingCount;
        public ushort Unknown1;
        public ushort Unknown2;
    }
}
