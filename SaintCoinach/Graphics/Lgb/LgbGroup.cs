using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Lgb {
    public class LgbGroup {
        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct HeaderData {
            public uint Unknown1;
            public int GroupNameOffset;
            public int EntriesOffset;
            public int EntryCount;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint Unknown9;
            public uint Unknown10;
        }
        #endregion

        #region Properties
        public LgbFile Parent { get; private set; }
        public HeaderData Header { get; private set; }
        public string Name { get; private set; }
        public ILgbEntry[] Entries { get; private set; }
        #endregion

        #region Constructor
        public LgbGroup(LgbFile parent, byte[] buffer, int offset) {
            this.Parent = parent;
            this.Header = buffer.ToStructure<HeaderData>(offset);
            this.Name = buffer.ReadString(offset + Header.GroupNameOffset);

            var entriesOffset = offset + Header.EntriesOffset;
            Entries = new ILgbEntry[Header.EntryCount];
            for(var i = 0; i < Header.EntryCount; ++i) {
                var entryOffset = entriesOffset + BitConverter.ToInt32(buffer, entriesOffset + i * 4);
                
                var type = (LgbEntryType)BitConverter.ToInt32(buffer, entryOffset);
                switch (type) {
                    case LgbEntryType.Model:
                        Entries[i] = new LgbModelEntry(Parent.File.Pack.Collection, buffer, entryOffset);
                        break;
                    case LgbEntryType.Gimmick:
                        Entries[i] = new LgbGimmickEntry(Parent.File.Pack.Collection, buffer, entryOffset);
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
