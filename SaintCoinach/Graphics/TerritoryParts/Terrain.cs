using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.TerritoryParts {
    public class Terrain {
        #region Properties
        public IO.File File { get; private set; }
        public TransformedModel[] Parts { get; private set; }
        #endregion

        #region Constructor
        public Terrain(IO.File file) {
            this.File = file;

            Build();
        }
        #endregion

        #region Build
        private void Build() {
            const int CountOffset = 0x04;
            const int SizeOffset = 0x08;
            const int BlockPositionsOffset = 0x34;
            const int BlockPositionSize = 0x04;

            var buffer = File.GetData();

            var blockCount = BitConverter.ToInt32(buffer, CountOffset);
            var blockSize = BitConverter.ToInt32(buffer, SizeOffset);

            var blockDirectory = File.Path.Substring(0, File.Path.LastIndexOf('/') + 1);

            Parts = new TransformedModel[blockCount];
            for (var i = 0; i < blockCount; ++i) {
                var blockPath = blockDirectory + string.Format("{0:D4}.mdl", i);
                var blockModelFile = (ModelFile)File.Pack.Collection.GetFile(blockPath);

                var x = BitConverter.ToInt16(buffer, BlockPositionsOffset + BlockPositionSize * i + 0);
                var z = BitConverter.ToInt16(buffer, BlockPositionsOffset + BlockPositionSize * i + 2);

                var translation = new Vector3 {
                    X = blockSize * (x + 0.5f),
                    Y = 0,
                    Z = blockSize * (z + 0.5f)
                };
                Parts[i] = new TransformedModel(blockModelFile.GetModelDefinition(), translation, Vector3.Zero, Vector3.One);
            }
        }
        #endregion
    }
}
