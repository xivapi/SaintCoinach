using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Sgb {
    public class SgbVfxEntry : ISgbGroupEntry {
        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct HeaderData {
            public SgbGroupEntryType Type;
            public uint UnknownId;
            public int NameOffset;
            public Vector3 Translation;
            public Vector3 Rotation;
            public Vector3 Scale;
            public int AvfxFileOffset;
            public int Unknown5_2;
            public int Unknown5_3;
            public int Unknown5_4;

            public int Unknown6;

            public Vector3 SomeVec3;

//            public int AvfxFileOffset;
            // unknowns
        }
        #endregion

        #region Properties
        public SgbGroupEntryType Type => Header.Type;
        public HeaderData Header { get; private set; }
        public string Name { get; private set; }
        public string AvfxFilePath { get; private set; }
        public IO.File AvfxFile { get; private set; }
        #endregion

        #region Constructor
        public SgbVfxEntry(IO.PackCollection packs, byte[] buffer, int offset) {
            this.Header = buffer.ToStructure<HeaderData>(offset);
            this.Name = buffer.ReadString(offset + Header.NameOffset);
            this.AvfxFilePath = buffer.ReadString(offset + Header.AvfxFileOffset);

            if (!string.IsNullOrEmpty(AvfxFilePath))
                this.AvfxFile = packs.GetFile(AvfxFilePath);
        }
        #endregion

    }
}
