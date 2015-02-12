using System;

namespace SaintCoinach.Graphics.ShPk {
    public class ShaderParameterReference {
        #region Static

        public const int Size = 0x10;

        #endregion

        #region Properties

        public ParameterType Type { get; private set; }
        public int Id { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public ShaderParameterReference(ParameterType type, byte[] buffer, int offset) {
            Type = type;
            Id = BitConverter.ToInt32(buffer, offset + 0x00);
        }

        #endregion

        #endregion
    }
}
