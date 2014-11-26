using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.ShPk {
    public class ShaderParameterReference {
        public const int Size = 0x10;

        #region Fields
        private ParameterType _Type;
        private int _Id;
        #endregion

        #region Properties
        public ParameterType Type { get { return _Type; } }
        public int Id { get { return _Id; } }
        #endregion

        #region Constructor
        public ShaderParameterReference(ParameterType type, byte[] buffer, int offset) {
            _Type = type;
            _Id = BitConverter.ToInt32(buffer, offset + 0x00);
        }
        #endregion
    }
}
