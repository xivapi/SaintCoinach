using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Pcb {
    public class PcbBlockData {
        public struct VertexI16 {
            ushort X;
            ushort Y;
            ushort Z;
        };
        public struct IndexData {
            byte Index1;
            byte Index2;
            byte Index3;
            byte Unknown1;
            byte Unknown2;
            byte Unknown3;
            byte Unknown4;
            byte Unknown5;
            byte Unknown6;
            byte Unknown7;
            byte Unknown8;
            byte Unknown9;
        };

        public Vector3[] Vertices;
        public VertexI16[] VerticesI16;
        public IndexData[] Indices;
    }
}
