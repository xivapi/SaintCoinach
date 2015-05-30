using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class PapAnimation {
        #region Properties
        public PapFile Parent { get; private set; }
        public string Name { get; private set; }
        public short Unknown20 { get; private set; }
        public int Index { get; private set; }
        public short Unknown26 { get; private set; }
        #endregion

        #region Constructor
        internal PapAnimation(PapFile file, byte[] buffer, ref int offset) {
            Parent = file;
            Name = buffer.ReadString(offset);

            offset += 0x20;
            Unknown20 = BitConverter.ToInt16(buffer, offset);

            offset += 0x02;
            Index = BitConverter.ToInt32(buffer, offset);

            offset += 0x04;
            Unknown26 = BitConverter.ToInt16(buffer, offset);

            offset += 0x02;
        }
        #endregion
    }
}
