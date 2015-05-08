using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.TerritoryParts {
    public class LgbFile {
        #region Fields
        private List<object> _Parts = new List<object>();
        #endregion

        #region Properties
        public IO.File File { get; private set; }
        public IEnumerable<object> Parts { get { return _Parts; } }
        #endregion

        #region Constructor
        public LgbFile(IO.File file) {
            this.File = file;

            Build();
        }
        #endregion

        #region Build
        private void Build() {
            const int BlockCountOffset = 0x20;
            const int BlockBaseOffset = 0x24;

            var buffer = File.GetData();

            var blockCount = BitConverter.ToInt32(buffer, BlockCountOffset);

            for (int i = 0; i < blockCount; ++i) {
                var blockOffset = BlockBaseOffset + BitConverter.ToInt32(buffer, BlockBaseOffset + i * 4);
                {
                    var part1Offset = blockOffset + BitConverter.ToInt32(buffer, blockOffset + 0x08);
                    var part1Count = BitConverter.ToInt32(buffer, blockOffset + 0x0C);

                    for (int j = 0; j < part1Count; ++j) {
                        var entryOffset = part1Offset + BitConverter.ToInt32(buffer, part1Offset + j * 4);

                        {
                            var type = BitConverter.ToInt32(buffer, entryOffset);

                            switch (type) {
                                case 1:
                                    _Parts.Add(new LgbModelEntry(File.Pack.Collection, buffer, entryOffset));
                                    break;
                                // TODO: Work out other parts.
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
