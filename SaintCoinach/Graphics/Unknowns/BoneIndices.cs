using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Unknowns {
    public struct BoneIndices {  // 8 in hsl, provides references to bones for MeshPartHeader
        public readonly int DataSize;
        public readonly ushort[] Bones;

        public int Size { get { return DataSize + 4; } }

        public BoneIndices(byte[] buffer, ref int offset) {
            DataSize = BitConverter.ToInt32(buffer, offset);
            offset += 4;
            Bones = new ushort[DataSize / 2];
            for (var i = 0; i < Bones.Length; ++i) {
                Bones[i] = BitConverter.ToUInt16(buffer, offset);
                offset += 2;
            }
        }
    }
}
