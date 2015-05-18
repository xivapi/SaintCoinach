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
            public uint Unknown2;
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
        public TransformedModel Model { get; private set; }
        #endregion

        #region Constructor
        public SgbModelEntry(IO.PackCollection packs, byte[] buffer, int offset) {
            this.Header = buffer.ToStructure<HeaderData>(offset);
            this.Name = buffer.ReadString(offset + Header.NameOffset);

            var mdlFilePath = buffer.ReadString(offset + Header.ModelFileOffset);
            if (!string.IsNullOrWhiteSpace(mdlFilePath)) {
                SaintCoinach.Graphics.ModelFile mdlFile = (SaintCoinach.Graphics.ModelFile)packs.GetFile(mdlFilePath);
                this.Model = new TransformedModel(mdlFile.GetModelDefinition(), Header.Translation, Header.Rotation, Header.Scale);
            }
        }
        #endregion
    }
}
