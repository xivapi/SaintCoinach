using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [StructLayout(LayoutKind.Sequential)]
    public struct MeshPartHeader {
        public int IndexOffset;
        public int IndexCount;
        public int AttributesMask;
        public short BoneReferenceOffset; // In ModelStruct8.Bones
        public short BoneReferenceCount;  // In ModelStruct8.Bones
    }
}
