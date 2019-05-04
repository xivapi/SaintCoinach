using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Sgb {

    public class SgbModelEntry : ISgbGroupEntry {
        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct HeaderData {
            public SgbGroupEntryType Type;
            public uint GimmickId;
            public int NameOffset;
            public Vector3 Translation;
            public Vector3 Rotation;
            public Vector3 Scale;
            public int ModelFileOffset;
            public int CollisionFileOffset;
        }
        #endregion

        #region Properties
        SgbGroupEntryType ISgbGroupEntry.Type { get { return Header.Type; } }
        public HeaderData Header { get; private set; }
        public string Name { get; private set; }
        public string ModelFilePath { get; private set; }
        public string CollisionFilePath { get; private set; }
        public TransformedModel Model { get; private set; }
        public Pcb.PcbFile CollisionFile { get; private set; }
        #endregion

        #region Constructor
        public SgbModelEntry(IO.PackCollection packs, byte[] buffer, int offset) {
            this.Header = buffer.ToStructure<HeaderData>(offset);
            this.Name = buffer.ReadString(offset + Header.NameOffset);

            ModelFilePath = buffer.ReadString(offset + Header.ModelFileOffset);
            CollisionFilePath = buffer.ReadString(offset + Header.CollisionFileOffset);

            if (!string.IsNullOrWhiteSpace(ModelFilePath)) {
                SaintCoinach.IO.File mdlFile;
                if (packs.TryGetFile(ModelFilePath, out mdlFile))
                    this.Model = new TransformedModel(((Graphics.ModelFile)mdlFile).GetModelDefinition(), Header.Translation, Header.Rotation, Header.Scale);
            }
            if (!string.IsNullOrWhiteSpace(CollisionFilePath)) {
                SaintCoinach.IO.File pcbFile;
                if (packs.TryGetFile(CollisionFilePath, out pcbFile))
                    this.CollisionFile = new Pcb.PcbFile(pcbFile);
            }
        }
        #endregion
    }
}
