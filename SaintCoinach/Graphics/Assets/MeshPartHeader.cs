using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Assets {
    public class MeshPartHeader {
        public const int Size = 0x10;

        #region Properties
        public int IndexOffset { get; private set; }
        public int IndexCount { get; private set; }
        public ushort Mask { get; private set; }
        #endregion

        #region Constructor
        internal MeshPartHeader(byte[] buffer, int offset) {
            IndexOffset = BitConverter.ToInt32(buffer, offset + 0x00);
            IndexCount = BitConverter.ToInt32(buffer, offset + 0x04);
            Mask = BitConverter.ToUInt16(buffer, offset + 0x08);
        }
        #endregion
    }
}
