using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.ShPk {
    public class ParameterHeader {
        public const int Size = 0x10;

        #region Fields
        private ParameterType _Type;
        private int _Id;
        private int _NameOffset;
        private int _NameLength;
        #endregion

        #region Properties
        public ParameterType Type { get { return _Type; } }
        public int Id { get { return _Id; } }
        public int NameOffset { get { return _NameOffset; } }
        public int NameLength { get { return _NameLength; } }
        #endregion

        #region Constructor
        public ParameterHeader(ParameterType type, byte[] buffer, int offset) {
            _Type = type;
            _Id = BitConverter.ToInt32(buffer, offset + 0x00);
            _NameOffset = BitConverter.ToInt32(buffer, offset + 0x04);
            _NameLength = BitConverter.ToInt32(buffer, offset + 0x08);
        }
        #endregion
    }
}
