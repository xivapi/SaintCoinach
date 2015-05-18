using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Lgb {
    public class LgbGimmickEntry : ILgbEntry {
        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct HeaderData {
            public LgbEntryType Type;
            public uint Unknown2;
            public int NameOffset;
            public Vector3 Translation;
            public Vector3 Rotation;
            public Vector3 Scale;
            public int GimmickFileOffset;
            // + 100 bytes of unknowns
        }
        #endregion

        #region Properties
        LgbEntryType ILgbEntry.Type { get { return Header.Type; } }
        public HeaderData Header { get; private set; }
        public string Name { get; private set; }
        public Sgb.SgbFile Gimmick { get; private set; }
        #endregion

        #region Constructor
        public LgbGimmickEntry(IO.PackCollection packs, byte[] buffer, int offset) {
            this.Header = buffer.ToStructure<HeaderData>(offset);
            this.Name = buffer.ReadString(offset + Header.NameOffset);

            var gimmickFilePath = buffer.ReadString(offset + Header.GimmickFileOffset);
            if (!string.IsNullOrWhiteSpace(gimmickFilePath)) {
                var file = packs.GetFile(gimmickFilePath);
                this.Gimmick = new Sgb.SgbFile(file);
            }
        }
        #endregion
    }
}
