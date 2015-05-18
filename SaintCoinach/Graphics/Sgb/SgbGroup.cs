using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Sgb {
    public class SgbGroup : ISgbData {
        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct HeaderData {
            public SgbDataType Type;
            public int NameOffset;
            public uint Unknown08;
            public uint Unknown0C;

            public uint Unknown10;
            public uint Unknown14;
            public uint Unknown18;
            public uint Unknown1C;

            public int EntryCount;
            public uint Unknown24;
            public uint Unknown28;
            public uint Unknown2C;

            public uint Unknown30;
            public uint Unknown34;
            public uint Unknown38;
            public uint Unknown3C;

            public uint Unknown40;
            public uint Unknown44;
        }
        #endregion

        #region Properties
        public HeaderData Header { get; private set; }
        SgbDataType ISgbData.Type { get { return Header.Type; } }
        public string Name { get; private set; }
        public SgbFile Parent { get; private set; }
        public ISgbGroupEntry[] Entries { get; private set; }
        #endregion

        #region Constructor
        public SgbGroup(SgbFile parent, byte[] buffer, int offset) {
            this.Parent = parent;

            var entriesOffset = offset;
            this.Header = buffer.ToStructure<HeaderData>(ref entriesOffset);
            this.Name = buffer.ReadString(offset + Header.NameOffset);

            Entries = new ISgbGroupEntry[Header.EntryCount];
            for(var i = 0; i < Header.EntryCount; ++i) {
                var entryOffset = entriesOffset + BitConverter.ToInt32(buffer, entriesOffset + i * 4);

                var type = (SgbGroupEntryType)BitConverter.ToInt32(buffer, entryOffset);
                switch (type) {
                    case SgbGroupEntryType.Model:
                        Entries[i] = new SgbModelEntry(Parent.File.Pack.Collection, buffer, entryOffset);
                        break;
                    default:
                        //System.Diagnostics.Trace.WriteLine(string.Format("{0}: Type {1} at 0x{2:X} in {3}", Parent.File.Path, type, entryOffset, Name));
                        break;
                        // TODO: Work out other parts.
                }
            }
        }
        #endregion
    }
}
