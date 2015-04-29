using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace SaintCoinach.Graphics {
    public class Map : ComponentContainer {
        #region Fields
        private string _BasePath;
        private ViewerEngine _Engine;
        private IO.PackCollection _PackCollection;
        private Xiv.TerritoryType _TerritoryType;
        #endregion

        #region Properties
        public string BasePath { get { return _BasePath; } }
        public Xiv.TerritoryType TerritoryType { get { return _TerritoryType; } }
        #endregion

        #region Constructor
        public Map(ViewerEngine engine, IO.PackCollection packCollection, Xiv.TerritoryType territoryType) {
            _Engine = engine;
            _PackCollection = packCollection;

            var bg = territoryType.Bg;
            var i = bg.IndexOf("/level/");
            if (i < 0)
                throw new NotSupportedException();
            _BasePath = bg.Substring(0, i);
            _TerritoryType = territoryType;

            PreProcess();
        }
        public Map(ViewerEngine engine, IO.PackCollection packCollection, string basePath) {
            _Engine = engine;
            _PackCollection = packCollection;
            _BasePath = basePath;

            PreProcess();
        }
        #endregion

        #region Pre
        private void PreProcess() {
            var terrainDir = string.Format("{0}/bgplate/", BasePath);

            if(_PackCollection.FileExists(terrainDir + Parts.BgPlate.TerrainIndexFile))
                Add(new Parts.BgPlate(_PackCollection, terrainDir));

            LoadLgb("bg.lgb");
        }
        private void LoadLgb(string fileName) {
            var fullPath = string.Format("{0}/level/{1}", BasePath, fileName);

            IO.File file;
            if (!_PackCollection.TryGetFile(fullPath, out file))
                return;

            var data = file.GetData();

            const int BlockBaseOffset = 0x24;
            var blockCount = BitConverter.ToInt32(data, 0x20);

            for (int i = 0; i < blockCount; ++i) {
                var blockOffset = BlockBaseOffset + BitConverter.ToInt32(data, BlockBaseOffset + i * 4);
                {
                    var part1Offset = blockOffset + BitConverter.ToInt32(data, blockOffset + 0x08);
                    var part1Count = BitConverter.ToInt32(data, blockOffset + 0x0C);

                    for (int j = 0; j < part1Count; ++j) {
                        var entryOffset = part1Offset + BitConverter.ToInt32(data, part1Offset + j * 4);

                        {
                            var type = BitConverter.ToInt32(data, entryOffset);

                            if (type == 1)
                                LoadType01LgbEntry(data, entryOffset, fileName);

                            /*
                            var b = new byte[0x50];
                            Array.Copy(data, entryOffset, b, 0, b.Length);
                            _Primitives.Add(Tuple.Create(
                                GeometricPrimitive.Cube.New(_Game.GraphicsDevice, _PrimitiveSize),
                                new Vector3(x, y, z),
                                entryOffset,
                                b));*/
                        }
                    }
                }
            }
        }
        private void LoadType01LgbEntry(byte[] data, int offset, string lgbFileName) {
            var x = BitConverter.ToSingle(data, offset + 0x0C);
            var y = BitConverter.ToSingle(data, offset + 0x10);
            var z = BitConverter.ToSingle(data, offset + 0x14);
            var rot1 = BitConverter.ToSingle(data, offset + 0x18);
            var rot2 = BitConverter.ToSingle(data, offset + 0x1C);
            var rot3 = BitConverter.ToSingle(data, offset + 0x20);
            var scaleX = BitConverter.ToSingle(data, offset + 0x24);
            var scaleY = BitConverter.ToSingle(data, offset + 0x28);
            var scaleZ = BitConverter.ToSingle(data, offset + 0x2C);

            var mdlOffset = offset + BitConverter.ToInt32(data, offset + 0x30);
            // Another one for collision is right after, but who cares about that!

            string modelFileName = null;

            const int MaxLength = 0x50;
            if (mdlOffset > 0 && mdlOffset < data.Length) {
                int resLen;
                var res = GetNullTerminatedString(data, out resLen, mdlOffset, MaxLength);
                if (resLen != MaxLength && res.EndsWith(".mdl"))
                    modelFileName = res;
            }

            if (modelFileName == null)
                return;

            var pos = new Vector3(x, y, z);
            var transform = Matrix.Scaling(scaleX, scaleY, scaleZ) * Matrix.RotationX(rot1) * Matrix.RotationY(rot2) * Matrix.RotationZ(rot3) * Matrix.Translation(pos);
  
            IO.File file;
            if (_PackCollection.TryGetFile(modelFileName, out file) && file is Assets.ModelFile) {
                var mdlFile = ((Assets.ModelFile)file);
                try {
                    var mdl = _Engine.ModelFactory.GetModel(mdlFile);
                    this.Add(new TransformedComponent(mdl, transform));
                } catch { }
            } else {
                //System.Diagnostics.Trace.WriteLine(string.Format("'{0}' Not found or not a model.", modelFileName), lgbFileName, offset);
            }
        }
        #endregion

        static string GetNullTerminatedString(byte[] self, int offset, int maxLength = int.MaxValue) {
            int nil;
            return GetNullTerminatedString(self, Encoding.UTF8, out nil, offset, maxLength);
        }
        static string GetNullTerminatedString(byte[] self, out int binaryLength, int offset, int maxLength = int.MaxValue) {
            return GetNullTerminatedString(self, Encoding.UTF8, out binaryLength, offset, maxLength);
        }
        static string GetNullTerminatedString(byte[] self, Encoding encoding, int offset, int maxLength = int.MaxValue) {
            int nil;
            return GetNullTerminatedString(self, encoding, out nil, maxLength);
        }
        static string GetNullTerminatedString(byte[] self, Encoding encoding, out int binaryLength, int offset, int maxLength = int.MaxValue) {
            if (self == null)
                throw new ArgumentNullException("self");
            if (offset < 0 || offset >= self.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            var start = offset;
            var end = start - 1;
            while (++end < self.Length && self[end] != 0 && (end - start) < maxLength) ;
            var len = Math.Min(end - start, maxLength);
            binaryLength = Math.Min(end - start + 1, maxLength);

            return encoding.GetString(self, start, len);
        }
    }
}
