using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [StructLayout(LayoutKind.Sequential)]
    public struct MaterialTextureParameter {
        public uint ParameterId;
        public ushort Unknown1;
        public ushort Unknown2;
        public int TextureIndex;
    }
}
