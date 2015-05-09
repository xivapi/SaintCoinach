using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [System.Diagnostics.DebuggerDisplay("{File.Path}")]
    public class Material {
        // TODO: One of the Unknowns most likely specifies what parts of /other/ models should be hidden
        //       Example: Gauntlets hiding rings or boots hiding the bottom of pants.
        private const string DummyTextureInMaterial = "dummy.tex";
        private const string DummyTexturePath = "common/graphics/texture/dummy.tex";

        #region Properties
        public MaterialDefinition Definition { get; private set; }
        public IO.File File { get; private set; }
        public ImcVariant Variant { get; private set; }

        public MaterialHeader Header { get; private set; }
        public Imaging.ImageFile[] TexturesFiles { get; private set; }
        public string[] Maps { get; private set; }
        public string[] DataSets { get; private set; }
        public string Shader { get; private set; }
        public byte[] Unknown1 { get; private set; }
        public byte[] Unknown2 { get; private set; }
        public MaterialMetadataHeader MetadataHeader { get; private set; }
        public Unknowns.MaterialStruct1[] UnknownStructs1 { get; private set; }
        public Unknowns.MaterialStruct2[] UnknownStructs2 { get; private set; }
        public MaterialTextureParameter[] TextureParameters { get; private set; }
        public byte[] Data { get; private set; }
        #endregion

        #region Constructor
        public Material(MaterialDefinition definition, IO.File file, ImcVariant variant) {
            this.Definition = definition;
            this.File = file;
            this.Variant = variant;

            Build();
        }
        #endregion

        #region Build
        private void Build() {
            var buffer = File.GetData();

            var offset = 0;

            this.Header = buffer.ToStructure<MaterialHeader>(ref offset);

            var stringsStart = offset + (4 * (Header.TextureCount + Header.MapCount + Header.DataSetCount));
            var texFileNames = ReadStrings(buffer, ref offset, stringsStart, Header.TextureCount);
            var packs = File.Pack.Collection;

            this.TexturesFiles = new Imaging.ImageFile[Header.TextureCount];
            for (var i = 0; i < Header.TextureCount; ++i) {
                var path = texFileNames[i];
                if (path == DummyTextureInMaterial)
                    path = DummyTexturePath;
                this.TexturesFiles[i] = (Imaging.ImageFile)packs.GetFile(path);
            }

            this.Maps = ReadStrings(buffer, ref offset, stringsStart, Header.MapCount);
            this.DataSets = ReadStrings(buffer, ref offset, stringsStart, Header.DataSetCount);

            this.Shader = buffer.ReadString(stringsStart + Header.ShaderOffset);

            offset = stringsStart + Header.StringsSize;

            this.Unknown1 = new byte[Header.UnknownSize1];
            Array.Copy(buffer, offset, this.Unknown1, 0, Header.UnknownSize1);
            offset += Header.UnknownSize1;

            this.Unknown2 = new byte[Header.UnknownSize2];
            Array.Copy(buffer, offset, this.Unknown2, 0, Header.UnknownSize2);
            offset += Header.UnknownSize2;

            this.MetadataHeader = buffer.ToStructure<MaterialMetadataHeader>(ref offset);

            this.UnknownStructs1 = buffer.ToStructures<Unknowns.MaterialStruct1>(MetadataHeader.UnknownStruct1Count, ref offset);
            this.UnknownStructs2 = buffer.ToStructures<Unknowns.MaterialStruct2>(MetadataHeader.UnknownStruct2Count, ref offset);
            this.TextureParameters = buffer.ToStructures<MaterialTextureParameter>(MetadataHeader.ParameterMappingCount, ref offset);

            this.Data = new byte[MetadataHeader.DataSize];
            Array.Copy(buffer, offset, this.Data, 0, MetadataHeader.DataSize);
        }
        private static string[] ReadStrings(byte[] buffer, ref int offsetOffset, int stringsOffset, int count) {
            var values = new string[count];
            for (var i = 0; i < count; ++i) {
                var o = BitConverter.ToInt16(buffer, offsetOffset);
                values[i] = buffer.ReadString(stringsOffset + o);
                offsetOffset += 4;
            }
            return values;
        }
        #endregion
    }
}
