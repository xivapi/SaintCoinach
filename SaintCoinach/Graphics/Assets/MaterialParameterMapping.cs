using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Assets {
    public class MaterialParameterMapping {
        public const int Size = 0x0C;

        #region Fields
        private int _Index;
        private uint _Id;
        #endregion

        #region Properties
        public int Index { get { return _Index; } }
        public uint Id { get { return _Id; } }
        #endregion

        #region Constructor
        public MaterialParameterMapping(byte[] buffer, int offset) {
            _Id = BitConverter.ToUInt32(buffer, offset + 0x00);
            _Index = BitConverter.ToInt32(buffer, offset + 0x08);
        }
        #endregion
    }
}
