using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Lgb {
    public class LgbEventObjectEntry : ILgbEntry {
        private static Ex.ExCollection Collection;
        private static Ex.ISheet EventObjectSheet;
        private static Ex.ISheet ExportedSgSheet;

        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct HeaderData {
            public LgbEntryType Type;
            public uint Unknown2;
            public int NameOffset;
            public Vector3 Translation;
            public Vector3 Rotation;
            public Vector3 Scale;
            public uint EventObjectId;
            public uint GimmickId;
            // + unknowns
        }
        #endregion

        #region Properties
        LgbEntryType ILgbEntry.Type { get { return Header.Type; } }
        public HeaderData Header { get; private set; }
        public string Name { get; private set; }
        public Sgb.SgbFile Gimmick { get; private set; }
        #endregion

        #region Constructor
        public LgbEventObjectEntry(IO.PackCollection packs, byte[] buffer, int offset) {
            this.Header = buffer.ToStructure<HeaderData>(offset);
            this.Name = buffer.ReadString(offset + Header.NameOffset);

            if (Collection == null) {
                LoadSheets(packs);
            }
            foreach (Ex.IDataRow row in EventObjectSheet) {
                if (row.Key == this.Header.EventObjectId) {
                    var sg = row.GetRaw(11);
                    foreach (Ex.IDataRow row2 in ExportedSgSheet) {
                        if (row2.Key == (ushort)sg) {
                            var path = ((SaintCoinach.Text.XivString)row2.GetRaw(0)).ToString();
                            if (!string.IsNullOrEmpty(path)) {
                                SaintCoinach.IO.File file;
                                if (packs.TryGetFile(path, out file))
                                    this.Gimmick = new Sgb.SgbFile(file);
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        private void LoadSheets(IO.PackCollection packs) {
            Collection = new Ex.ExCollection(packs);
            EventObjectSheet = Collection.GetSheet("EObj");
            ExportedSgSheet = Collection.GetSheet("ExportedSG");

        }
        #endregion
    }
}
