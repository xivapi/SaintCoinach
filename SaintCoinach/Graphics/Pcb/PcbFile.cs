using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Pcb
{
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
        public PcbBlockEntry[] Data { get; private set;}
        #endregion

        #region Constructor
        public PcbFile(IO.File file)
        {
            this.File = file;

            Build();
        }
        #endregion

        #region Build
        private void Build()
        {
            var buffer = File.GetData();
            int entryOffset = 0x18;

            this.Header = buffer.ToStructure<HeaderData>(0);
            var data = new List<PcbBlockEntry>(Header.EntryCount);
            bool isGroup = true;
            while (isGroup) {
                PcbBlockEntry entry = new PcbBlockEntry(this, buffer, ref entryOffset);

            }
        }
        #endregion
    }
}
