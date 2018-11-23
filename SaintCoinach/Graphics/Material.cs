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
        public byte[] Unknown { get; private set; }
        public byte[] DataSetData { get; private set; }
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

            this.Unknown = new byte[Header.UnknownSize];
            Array.Copy(buffer, offset, this.Unknown, 0, Header.UnknownSize);
            offset += Header.UnknownSize;

            this.DataSetData = new byte[Header.DataSetSize];
            Array.Copy(buffer, offset, this.DataSetData, 0, Header.DataSetSize);
            offset += Header.DataSetSize;

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

        public unsafe System.Drawing.Image GetColorSet() {
            // ColorSet is R16G16B16A16F.
            const int width = 4;
            const int height = 16;

            var bmp = new System.Drawing.Bitmap(width, height);
            var i = 0;
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    var offset = i++ * 4 * 2;

                    var red = GetColorValue(HalfHelper.Unpack(DataSetData, offset));
                    var green = GetColorValue(HalfHelper.Unpack(DataSetData, offset + 2));
                    var blue = GetColorValue(HalfHelper.Unpack(DataSetData, offset + 4));
                    var alpha = GetColorValue(HalfHelper.Unpack(DataSetData, offset + 6));
                    bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(alpha, red, green, blue));
                }
            }

            return bmp;
        }

        private static byte GetColorValue(float half) {
            if (half > 1)
                return byte.MaxValue;
            if (half < 0)
                return 0;
            return (byte)(half * byte.MaxValue);
        }
    }
}
