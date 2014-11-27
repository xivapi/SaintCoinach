using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Assets {
    public class MaterialVersion {
        const string DummyTextureInMaterial = "dummy.tex";
        const string DummyTexturePath = "common/graphics/texture/dummy.tex";

        #region Fields
        private int _Version;
        private Material _Material;
        private bool _CanStain;

        private string _BasePath;
        private string _StainedFormat;

        private IO.File _CurrentFile;

        private int _CurrentStain = 0;
        private IReadOnlyList<Imaging.ImageFile> _Textures;
        private IReadOnlyList<string> _Maps;
        private IReadOnlyList<string> _DataSets;
        private int[] _AvailableStains;
        private string _Shader;
        private MaterialParameterMapping[] _ParameterMappings;

        private IO.PackCollection _PackCollection;
        #endregion

        #region Properties
        public int Version { get { return _Version; } }
        public Material Material { get { return _Material; } }

        public IO.File CurrentFile { get { return _CurrentFile; } }

        public bool CanStain { get { return _CanStain; } }
        public IEnumerable<int> AvailableStains { get { return _AvailableStains; } }
        public int CurrentStain {
            get { return _CurrentStain; }
            set {
                if (!CanStain)
                    throw new NotSupportedException();
                _CurrentStain = value;
                Load();
            }
        }
        public IReadOnlyList<Imaging.ImageFile> Textures {
            get { return _Textures; }
        }
        public IReadOnlyList<string> Maps {
            get { return _Maps; }
        }
        public IReadOnlyList<string> DataSets {
            get { return _DataSets; }
        }
        public string Shader {
            get { return _Shader; }
        }
        public IEnumerable<MaterialParameterMapping> ParameterMappings {
            get { return _ParameterMappings; }
        }
        #endregion

        #region Constructor
        public MaterialVersion(Material material, int version, string path) {
            _Material = material;
            _BasePath = path;
            _Version = version;

            _PackCollection = Material.Model.File.Directory.Pack.Collection;

            Load();
            CheckForStaining();
        }
        #endregion

        #region Load
        private void CheckForStaining() {
            var lastSep = _BasePath.LastIndexOf('/');
            var ext = _BasePath.LastIndexOf('.');

            _StainedFormat = string.Format(
                "{0}/staining{1}_s{{0:D4}}{2}",
                _BasePath.Substring(0, lastSep),
                _BasePath.Substring(lastSep, ext - lastSep),
                _BasePath.Substring(ext));

            const int Minimum = 1;
            const int Maximum = 9999;

            var stains = new List<int>();

            for (int i = Minimum; i <= Maximum; ++i) {
                var f = string.Format(_StainedFormat, i);
                if (_PackCollection.FileExists(f))
                    stains.Add(i);
            }

            _CanStain = stains.Count > 0;
            _AvailableStains = stains.ToArray();
        }
        private void Load() {
            if (CurrentStain == 0)
                _CurrentFile = _PackCollection.GetFile(_BasePath);
            else {
                var path = string.Format(_StainedFormat, CurrentStain);
                _CurrentFile = _PackCollection.GetFile(path);
            }

            var buffer = _CurrentFile.GetData();
            Read(buffer);
        }
        private void Read(byte[] buffer) {
            const int TextureCountOffset = 0x0C;
            const int MapCountOffset = 0x0D;
            const int DataSetCountOffset = 0x0E;

            const int InfoOffset = 0x10;
            const int TextureInfoLength = 0x04;
            const int MapInfoLength = 0x04;
            const int DataSetInfoLength = 0x04;

            var textureCount = buffer[TextureCountOffset];
            var mapCount = buffer[MapCountOffset];
            var dataSetCount = buffer[DataSetCountOffset];

            var offset = InfoOffset;
            offset += textureCount * TextureInfoLength;
            offset += mapCount * MapInfoLength;
            offset += dataSetCount * DataSetInfoLength;

            ReadShader(buffer, offset);

            var currentInfoOffset = InfoOffset;
            ReadTextures(buffer, currentInfoOffset, offset, textureCount);
            currentInfoOffset += textureCount * TextureInfoLength;

            ReadMaps(buffer, currentInfoOffset, offset, mapCount);
            currentInfoOffset += mapCount * MapInfoLength;

            ReadDataSets(buffer, currentInfoOffset, offset, dataSetCount);

            const int StringsLengthOffset = 0x08;
            var stringsLength = BitConverter.ToInt16(buffer, StringsLengthOffset);
            offset += stringsLength;

            ReadAdditional(buffer, offset);
        }
        private void ReadShader(byte[] buffer, int dataOffset) {
            const int ShaderPositionOffset = 0x0A;

            var shaderOffset = BitConverter.ToUInt16(buffer, ShaderPositionOffset);
            _Shader = GetNullTerminatedString(buffer, dataOffset + shaderOffset);
        }

        private void ReadTextures(byte[] buffer, int infoOffset, int dataOffset, int count) {
            var offsets = new int[count];
            for (int i = 0; i < count; ++i)
                offsets[i] = BitConverter.ToUInt16(buffer, infoOffset + 0x04 * i);

            var textures = new List<Imaging.ImageFile>(count);
            foreach (var offset in offsets) {
                var actualOffset = dataOffset + offset;
                var str = GetNullTerminatedString(buffer, actualOffset);
                if (DummyTextureInMaterial.Equals(str, StringComparison.OrdinalIgnoreCase))
                    str = DummyTexturePath;
                IO.File file;
                if (_PackCollection.TryGetFile(str, out file)) {
                    if (!(file is Imaging.ImageFile))
                        System.Diagnostics.Trace.WriteLine(string.Format("Material<{0}>: Texture '{1}' is not an image.", _CurrentFile.Path, str));
                } else {
                    System.Diagnostics.Trace.WriteLine(string.Format("Material<{0}>: Texture '{1}' could not be found.", _CurrentFile.Path, str));
                    file = null;
                }

                textures.Add(file as Imaging.ImageFile);
            }

            _Textures = new ReadOnlyCollection<Imaging.ImageFile>(textures);
        }

        private void ReadMaps(byte[] buffer, int infoOffset, int dataOffset, int count) {
            var offsets = new int[count];
            for (int i = 0; i < count; ++i)
                offsets[i] = BitConverter.ToUInt16(buffer, infoOffset + 0x04 * i);

            var maps = new List<string>();
            foreach (var offset in offsets) {
                var actualOffset = dataOffset + offset;
                maps.Add(GetNullTerminatedString(buffer, actualOffset));
            }
            _Maps = new ReadOnlyCollection<string>(maps);
        }

        private void ReadDataSets(byte[] buffer, int infoOffset, int dataOffset, int count) {
            var offsets = new int[count];
            for (int i = 0; i < count; ++i)
                offsets[i] = BitConverter.ToUInt16(buffer, infoOffset + 0x04 * i);

            var dataSets = new List<string>();
            foreach (var offset in offsets) {
                var actualOffset = dataOffset + offset;
                dataSets.Add(GetNullTerminatedString(buffer, actualOffset));
            }
            _DataSets = new ReadOnlyCollection<string>(dataSets);
        }

        private void ReadAdditional(byte[] buffer, int offset) {
            var c1 = BitConverter.ToInt16(buffer, offset + 0x06);
            var c2 = BitConverter.ToInt16(buffer, offset + 0x08);
            var mappingCount = BitConverter.ToInt16(buffer, offset + 0x0A);

            offset += 0x10;
            offset += c1 * 0x08;
            offset += c2 * 0x08;

            _ParameterMappings = new MaterialParameterMapping[mappingCount];
            for (var i = 0; i < mappingCount; ++i) {
                _ParameterMappings[i] = new MaterialParameterMapping(buffer, offset);
                offset += MaterialParameterMapping.Size;
            }
        }

        private static string GetNullTerminatedString(byte[] buffer, int offset) {
            var end = offset - 1;
            while (++end < buffer.Length && buffer[end] != 0) ;
            var len = end - offset;
            return Encoding.ASCII.GetString(buffer, offset, len);
        }
        #endregion

        public override string ToString() {
            return _BasePath;
        }
    }
}
