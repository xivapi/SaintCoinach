using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Sgb {

    public class SgbGimmickEntry : ISgbGroupEntry {
        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct HeaderData {
            public SgbGroupEntryType Type;
            public uint GimmickId;
            public int NameOffset;
            public Vector3 Translation;
            public Vector3 Rotation;
            public Vector3 Scale;
            public int GimmickFileOffset;
            public int CollisionFileOffset;
            // SgbGimmickEntry size is around 152 bytes?
        }
        #endregion

        #region Properties
        SgbGroupEntryType ISgbGroupEntry.Type { get { return Header.Type; } }
        public HeaderData Header { get; private set; }
        public string Name { get; private set; }
        public SgbFile Gimmick { get; private set; }
        #endregion

        #region Constructor
        public SgbGimmickEntry(IO.PackCollection packs, byte[] buffer, int offset) {
            this.Header = buffer.ToStructure<HeaderData>(offset);
            this.Name = buffer.ReadString(offset + Header.NameOffset);

            var sgbFileName = buffer.ReadString(offset + Header.GimmickFileOffset);
            if (!string.IsNullOrWhiteSpace(sgbFileName)) {
                SaintCoinach.IO.File file;
                if (packs.TryGetFile(sgbFileName, out file))
                    this.Gimmick = new SgbFile(file);
            }
        }
        #endregion
    }
}
