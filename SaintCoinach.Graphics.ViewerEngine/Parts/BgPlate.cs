using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace SaintCoinach.Graphics.Parts {
    public class BgPlate : ComponentContainer {
        static readonly uint TerrainIndexFile = IO.Hash.Compute("terrain.tera");

        #region Fields
        private IO.Directory _TerrainDirectory;
        #endregion

        #region Properties
        public IO.Directory TerrainDirectory { get { return _TerrainDirectory; } }
        #endregion

        #region Constructor
        public BgPlate(IO.Directory terrainDirectory) {
            _TerrainDirectory = terrainDirectory;

            IO.File terrainIndex = terrainDirectory.GetFile(TerrainIndexFile);
            var terrainIndexData = terrainIndex.GetData();

            var blockCount = BitConverter.ToInt32(terrainIndexData, 0x04);
            var blockSize = BitConverter.ToInt32(terrainIndexData, 0x08);

            for (var block = 0; block < blockCount; ++block) {
                var fileKey = IO.Hash.Compute(string.Format("{0:D4}.mdl", block));
                if (!terrainDirectory.FileExists(fileKey))
                    continue;
                var blockFile = (Assets.ModelFile)terrainDirectory.GetFile(fileKey);

                var x = BitConverter.ToInt16(terrainIndexData, 0x34 + 4 * block + 0);
                var z = BitConverter.ToInt16(terrainIndexData, 0x34 + 4 * block + 2);

                var translation = new Vector3(blockSize * (x + 0.5f), 0, blockSize * (z + 0.5f));
                Add(new BgPlateBlock(blockFile, new Point(x, z), translation));
            }
        }
        #endregion
    }
}
