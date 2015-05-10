using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.TerritoryParts {
    public class LgbModelEntry {
        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct HeaderData {
            public uint Type;
            public uint Unknown2;
            public uint Unknown3;
            public Vector3 Translation;
            public Vector3 Rotation;
            public Vector3 Scale;
            public int ModelFileOffset;
            public int CollisionFileOffset;
        }
        #endregion

        #region Properties
        public HeaderData Header { get; private set; }
        public TransformedModel Model { get; private set; }
        #endregion

        #region Constructor
        public LgbModelEntry(IO.PackCollection packs, byte[] buffer, int offset) {
            this.Header = buffer.ToStructure<HeaderData>(offset);

            var mdlFilePath = buffer.ReadString(offset + Header.ModelFileOffset);
            if (!string.IsNullOrWhiteSpace(mdlFilePath)) {
                SaintCoinach.Graphics.ModelFile mdlFile = (SaintCoinach.Graphics.ModelFile)packs.GetFile(mdlFilePath);
                this.Model = new TransformedModel(mdlFile.GetModelDefinition(), Header.Translation, Header.Rotation, Header.Scale);
            }
        }
        #endregion
    }
}
