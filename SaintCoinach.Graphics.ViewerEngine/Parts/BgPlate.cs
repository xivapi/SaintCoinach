using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace SaintCoinach.Graphics.Parts {
    public class BgPlate : ComponentContainer {
        public static readonly string TerrainIndexFile = "terrain.tera";

        #region Fields
        private string _TerrainDirectory;
        #endregion

        #region Properties
        public string TerrainDirectory { get { return _TerrainDirectory; } }
        #endregion

        #region Constructor
        public BgPlate(IO.PackCollection pack, string terrainDirectory) {
            _TerrainDirectory = terrainDirectory;

            IO.File terrainIndex = pack.GetFile(TerrainDirectory + TerrainIndexFile);
            var terrainIndexData = terrainIndex.GetData();

            var blockCount = BitConverter.ToInt32(terrainIndexData, 0x04);
            var blockSize = BitConverter.ToInt32(terrainIndexData, 0x08);

            for (var block = 0; block < blockCount; ++block) {
                var filePath = terrainDirectory + string.Format("{0:D4}.mdl", block);
                IO.File file;
                if (!pack.TryGetFile(filePath, out file))
                    continue;
                var blockFile = (Assets.ModelFile)file;

                var x = BitConverter.ToInt16(terrainIndexData, 0x34 + 4 * block + 0);
                var z = BitConverter.ToInt16(terrainIndexData, 0x34 + 4 * block + 2);

                var translation = new Vector3(blockSize * (x + 0.5f), 0, blockSize * (z + 0.5f));
                Add(new BgPlateBlock(blockFile, new Point(x, z), translation));
            }
        }
        #endregion
    }
}
