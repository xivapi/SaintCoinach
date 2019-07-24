using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Lgb {
    public class LgbModelEntry : ILgbEntry {
        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct HeaderData {
            public LgbEntryType Type;
            public uint Unknown2;
            public int NameOffset;
            public Vector3 Translation;
            public Vector3 Rotation;
            public Vector3 Scale;
            public int ModelFileOffset;
            public int CollisionFileOffset;
            public int Unknown4;
            public int Unknown5;
            public int Unknown6;
            public int Unknown7;
            public int Unknown8;
            public int Unknown9;
        }
        #endregion

        #region Properties
        LgbEntryType ILgbEntry.Type { get { return Header.Type; } }
        public HeaderData Header { get; private set; }
        public string Name { get; private set; }
        public string ModelFilePath { get; private set; }
        public string CollisionFilePath { get; private set; }
        public TransformedModel Model { get; private set; }
        public Pcb.PcbFile CollisionFile { get; private set; }
        #endregion

        #region Constructor
        public LgbModelEntry(IO.PackCollection packs, byte[] buffer, int offset) {
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
                try
                {
                    SaintCoinach.IO.File pcbFile;
                    if (packs.TryGetFile(CollisionFilePath, out pcbFile))
                        this.CollisionFile = new Pcb.PcbFile(pcbFile);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{Name} at 0x{offset:X} PcbFile failure: {ex.Message}");
                }
            }
        }
        #endregion
    }
}
