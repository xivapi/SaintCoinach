using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Pcb
{
    // TODO: This class should be reworked and checked for errors - the problems they cause with certain PCBs are meanwhile caught in LgbModelEntry L55+
    public class PcbFile
    {
        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct HeaderData {
            public uint Unknown1;
            public uint Unknown2;
            public int EntryCount;
            public int IndicesCount;
            public ulong Padding;
        }
        #endregion

        #region Properties
        public HeaderData Header { get; private set; }
        public IO.File File { get; private set; }
        public List<PcbBlockEntry> Data { get; private set;}
        #endregion

        #region Constructor
        public PcbFile(IO.File file)
        {
            this.File = file;

            // todo: fix this
            //Build();
        }
        #endregion

        #region Build
        private void Build()
        {
            var buffer = File.GetData();
            int entryOffset = 0x18;

            this.Header = buffer.ToStructure<HeaderData>(0);
            Data = new List<PcbBlockEntry>(Header.EntryCount);
            bool isGroup = true;
            while (isGroup) {
                PcbBlockEntry entry = new PcbBlockEntry(this, buffer, entryOffset);

                if (isGroup = (entry.Header.Type == PcbBlockDataType.Group)) {
                    ParsePcbBlockEntry(buffer, entryOffset + 0x30, entryOffset);
                    entryOffset += (int)entry.Header.BlockSize;
                }
                else {
                    ParsePcbBlockEntry(buffer, entryOffset, entryOffset);
                }
            }
        }
        #endregion

        #region Functions
        private void ParsePcbBlockEntry(byte[] buffer, int entryOffset, int globalOffset) {
            int offset = 0;
            bool isGroup = true;
            while (isGroup) {
                PcbBlockEntry entry = new PcbBlockEntry(this, buffer, entryOffset);

                if (isGroup = (entry.Header.Type == PcbBlockDataType.Group)) {
                    ParsePcbBlockEntry(buffer, entryOffset + offset + 0x30, globalOffset + offset + entryOffset);
                    offset += (int)entry.Header.BlockSize;
                }
                else {
                    var dOffset = Marshal.SizeOf<PcbBlockEntry.HeaderData>() + offset;
                    var blockSize = Marshal.SizeOf<PcbBlockEntry.HeaderData>() +
                        entry.Header.VerticesCount * 3 * 4 +
                        entry.Header.VerticesI16Count * 3 * 2 +
                        entry.Header.IndicesCount * 3 * 2;

                    if (entry.Header.VerticesCount != 0) {
                        entry.Data.Vertices = new Vector3[entry.Header.VerticesCount];
                        for (var i = 0; i < entry.Header.VerticesCount; ++i) {
                            entry.Data.Vertices[i] = buffer.ToStructure<Vector3>(ref dOffset);
                        }
                    }
                    if (entry.Header.VerticesI16Count != 0) {
                        entry.Data.VerticesI16 = new PcbBlockData.VertexI16[entry.Header.VerticesI16Count];
                        for (var i = 0; i < entry.Header.VerticesI16Count; ++i) {
                            entry.Data.VerticesI16[i] = buffer.ToStructure<PcbBlockData.VertexI16>(ref dOffset);
                        }
                    }
                    if (entry.Header.IndicesCount != 0) {
                        entry.Data.Indices = new PcbBlockData.IndexData[entry.Header.IndicesCount];
                        for (var i = 0; i < entry.Header.IndicesCount; ++i) {
                            entry.Data.Indices[i] = buffer.ToStructure<PcbBlockData.IndexData>(ref dOffset);
                        }
                    }
                    Data.Add(entry);
                }
            }
        }
        #endregion
    }
}
