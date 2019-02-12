using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Pcb {
    public class PcbBlockEntry {
        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct HeaderData
        {
            public PcbBlockDataType Type;       // 0 for entry, 0x30 for group
            public uint BlockSize;       // when group size in bytes for the group block
            public Vector3 Min;          // bounding box
            public Vector3 Max;
            public ushort VerticesI16Count; // number of vertices packed into 16 bit
            public ushort IndicesCount;  // number of indices
            public uint VerticesCount;   // number of normal float vertices
        }
        #endregion

        #region Properties
        public HeaderData Header { get; private set; }
        public PcbFile Parent { get; private set; }
        public PcbBlockData Data { get; private set; }
        public int Offset { get; private set; }
        #endregion

        #region Constructor
        public PcbBlockEntry(PcbFile parent, byte[] buffer, int offset)
        {
            this.Parent = parent;
            Offset = offset;
            this.Header = buffer.ToStructure<HeaderData>(ref offset);
            this.Data = new PcbBlockData();
            Build(buffer);
        }
        #endregion
        #region Functions
        private void ParseBlockEntry(byte[] buffer, int offset, int globalOffset) {

        }
        #endregion
        #region Build
        private void Build(byte[] buffer)
        {

        }
        #endregion
    }
}
