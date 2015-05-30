using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class ModelDefinition {
        public const int StringsCountOffset = 0x00;
        public const int StringsSizeOffset = 0x04;
        public const int StringsOffset = 0x08;

        public const int ModelCount = 3;

        #region Fields
        internal string[] MaterialNames;
        internal string[] AttributeNames;
        private Model[] _Models = new Model[ModelCount];
        #endregion

        #region Properties
        public IEnumerable<ModelQuality> AvailableQualities { get; private set; }
        public ModelDefinitionHeader Header { get; private set; }
        public ModelFile File { get; private set; }
        public VertexFormat[] VertexFormats { get; private set; }
        public Unknowns.ModelStruct1[] UnknownStructs1 { get; private set; }
        public ModelHeader[] ModelHeaders { get; private set; }
        public MeshHeader[] MeshHeaders { get; private set; }
        public ModelAttribute[] Attributes { get; private set; }
        public Unknowns.ModelStruct2[] UnknownStructs2 { get; private set; }
        public MeshPartHeader[] MeshPartHeaders { get; private set; }
        public Unknowns.ModelStruct3[] UnknownStructs3 { get; private set; }
        public MaterialDefinition[] Materials { get; private set; }
        public string[] BoneNames { get; private set; }
        public Unknowns.BoneList[] BoneLists { get; private set; }
        public Unknowns.ModelStruct5[] UnknownStructs5 { get; private set; }
        public Unknowns.ModelStruct6[] UnknownStructs6 { get; private set; }
        public Unknowns.ModelStruct7[] UnknownStructs7 { get; private set; }
        public Unknowns.BoneIndices BoneIndices { get; private set; }
        // Here's padding, but not keeping a variable amount of 0s
        public ModelBoundingBoxes BoundingBoxes { get; private set; }
        public Bone[] Bones { get; private set; }
        #endregion

        #region Constructor
        public ModelDefinition(ModelFile file) {
            File = file;

            Build();
        }
        #endregion

        #region Get
        public Model GetModel(int quality) { return GetModel((ModelQuality)quality); }
        public Model GetModel(ModelQuality quality) {
            var v = (int)quality;
            if (_Models[v] == null)
                _Models[v] = new Model(this, quality);
            return _Models[v];
        }
        #endregion

        #region Build
        private void Build() {
            const int DefinitionPart = 1;

            var buffer = File.GetPart(DefinitionPart);
            var stringsSize = BitConverter.ToInt32(buffer, StringsSizeOffset);

            var offset = StringsOffset + stringsSize;    // Skipping those, they'll be read further along the road

            this.Header = buffer.ToStructure<ModelDefinitionHeader>(ref offset);
            this.UnknownStructs1 = buffer.ToStructures<Unknowns.ModelStruct1>(Header.UnknownStruct1Count, ref offset);
            this.ModelHeaders = buffer.ToStructures<ModelHeader>(ModelCount, ref offset);

            var availableQualities = new List<ModelQuality>();
            for (var i = 0; i < this.ModelHeaders.Length; ++i) {
                if (this.ModelHeaders[i].MeshCount > 0)
                    availableQualities.Add((ModelQuality)i);
            }
            this.AvailableQualities = availableQualities;

            this.MeshHeaders = buffer.ToStructures<MeshHeader>(Header.MeshCount, ref offset);

            this.AttributeNames = ReadStrings(buffer, Header.AttributeCount, ref offset);
            this.Attributes = new ModelAttribute[Header.AttributeCount];
            for (var i = 0; i < Header.AttributeCount; ++i)
                this.Attributes[i] = new ModelAttribute(this, i);

            this.UnknownStructs2 = buffer.ToStructures<Unknowns.ModelStruct2>(Header.UnknownStruct2Count, ref offset);
            this.MeshPartHeaders = buffer.ToStructures<MeshPartHeader>(Header.PartCount, ref offset);
            this.UnknownStructs3 = buffer.ToStructures<Unknowns.ModelStruct3>(Header.UnknownStruct3Count, ref offset);

            this.MaterialNames = ReadStrings(buffer, Header.MaterialCount, ref offset);
            this.Materials = new MaterialDefinition[Header.MaterialCount];
            for (var i = 0; i < Header.MaterialCount; ++i)
                this.Materials[i] = new MaterialDefinition(this, i);


            this.BoneNames = ReadStrings(buffer, Header.BoneCount, ref offset);
            this.BoneLists = buffer.ToStructures<Unknowns.BoneList>(Header.UnknownStruct4Count, ref offset);
            this.UnknownStructs5 = buffer.ToStructures<Unknowns.ModelStruct5>(Header.UnknownStruct5Count, ref offset);
            this.UnknownStructs6 = buffer.ToStructures<Unknowns.ModelStruct6>(Header.UnknownStruct6Count, ref offset);
            this.UnknownStructs7 = buffer.ToStructures<Unknowns.ModelStruct7>(Header.UnknownStruct7Count, ref offset);
            this.BoneIndices = new Unknowns.BoneIndices(buffer, ref offset);

            offset += buffer[offset] + 1;   // Just padding, first byte specifying how many 0-bytes follow.

            this.BoundingBoxes = buffer.ToStructure<ModelBoundingBoxes>(ref offset);

            this.Bones = new Bone[Header.BoneCount];
            for (var i = 0; i < Header.BoneCount; ++i)
                this.Bones[i] = new Bone(this, i, buffer, ref offset);

            if (offset != buffer.Length) {
                //System.Diagnostics.Debugger.Break();    // Something's not right here.
            }

            BuildVertexFormats();
        }
        private void BuildVertexFormats() {
            const int FormatPart = 0;

            var buffer = File.GetPart(FormatPart);

            this.VertexFormats = new VertexFormat[Header.MeshCount];
            var offset = 0;
            for (var i = 0; i < Header.MeshCount; ++i)
                this.VertexFormats[i] = new VertexFormat(buffer, ref offset);
        }
        private static string[] ReadStrings(byte[] buffer, int count, ref int offset) {
            var values = new string[count];
            for(var i = 0; i < count; ++i) {
                var stringOffset = BitConverter.ToInt32(buffer, offset);
                values[i] = buffer.ReadString(StringsOffset + stringOffset);
                offset += 4;
            }
            return values;
        }
        #endregion
    }
}
