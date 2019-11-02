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

        [StructLayout(LayoutKind.Sequential)]
        public struct Offset1CHeaderData {
            public SgbDataType Type;
            public int NameOffset;
            public uint Unknown08;

            public int EntryCount;
            public uint Unknown14;
            public int ModelFileOffset;
            public Vector3 UnknownFloat3;
            public Vector3 UnknownFloat3_2;
            public int StateOffset;
            public int ModelFileOffset2;
            public uint Unknown3;
            public float Unknown4;
            public int NameOffset2;
            public Vector3 UnknownFloat3_3;
        }
        #endregion

        #region Properties
        public HeaderData Header { get; private set; }
        public Offset1CHeaderData Offset1CHeader { get; private set; }
        SgbDataType ISgbData.Type { get { return Header.Type; } }
        public string Name, ModelFile1, ModelFile2, ModelFile3;// { get; private set; }
        public List<string> States;
        public SgbFile Parent { get; private set; }
        public ISgbGroupEntry[] Entries { get; private set; }
        #endregion

        #region Constructor
        
        public SgbGroup(SgbFile parent, byte[] buffer, int offset, bool isOffset1C = false) {
            this.Parent = parent;

            var entriesOffset = offset;
            int count = 0;
            if (isOffset1C) {
                this.Header = buffer.ToStructure<HeaderData>(entriesOffset);
                this.Offset1CHeader = buffer.ToStructure<Offset1CHeaderData>(ref entriesOffset);
                this.Name = buffer.ReadString(offset + Offset1CHeader.NameOffset);
                this.ModelFile1 = buffer.ReadString(offset + Offset1CHeader.ModelFileOffset + 1);
                this.ModelFile2 = buffer.ReadString(offset + Offset1CHeader.ModelFileOffset2 + 1);
                this.ModelFile3 = buffer.ReadString(offset + Offset1CHeader.NameOffset2 + 64);

                count = Offset1CHeader.EntryCount;
                Entries = new ISgbGroupEntry[count];

                for (var i = 0; i < count; ++i) {
                    try {
                        var entryOffset = entriesOffset + (i * 24);
                        Entries[i] = new SgbGroup1CEntry(parent.File.Pack.Collection, buffer, entryOffset);
                        break;
                    }
                    catch (Exception e) {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                }
            }
            else {
                this.Header = buffer.ToStructure<HeaderData>(ref entriesOffset);
                this.Name = buffer.ReadString(offset + Header.NameOffset);
                count = Header.EntryCount;
                Entries = new ISgbGroupEntry[count];

                for (var i = 0; i < count; ++i) {
                    try {
                        var entryOffset = entriesOffset + BitConverter.ToInt32(buffer, entriesOffset + i * 4);

                        var type = (SgbGroupEntryType)BitConverter.ToInt32(buffer, entryOffset);
                        var typeStr = ((Lgb.LgbEntryType)type).ToString();
                        switch (type) {
                            case SgbGroupEntryType.Model:
                                Entries[i] = new SgbModelEntry(Parent.File.Pack.Collection, buffer, entryOffset);
                                break;
                            case SgbGroupEntryType.Gimmick:
                                Entries[i] = new SgbGimmickEntry(Parent.File.Pack.Collection, buffer, entryOffset);
                                break;
                            case SgbGroupEntryType.Light:
                                Entries[i] = new SgbLightEntry(Parent.File.Pack.Collection, buffer, entryOffset);
                                break;
                            case SgbGroupEntryType.Vfx:
                                Entries[i] = new SgbVfxEntry(Parent.File.Pack.Collection, buffer, entryOffset);
                                break;
                            default:
                                //System.Diagnostics.Trace.WriteLine(string.Format("{0}: Type {1} at 0x{2:X} in {3}", Parent.File.Path, type, entryOffset, Name));
                                break;
                                // TODO: Work out other parts.
                        }
                    }
                    catch (Exception e) {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                }
            }
        }
        #endregion
    }
}
