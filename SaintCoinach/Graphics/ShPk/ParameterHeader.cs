using System;

namespace SaintCoinach.Graphics.ShPk {
    public class ParameterHeader {
        #region Static

        public const int Size = 0x10;

        #endregion

        #region Properties

        public ParameterType Type { get; private set; }
        public int Id { get; private set; }
        public int NameOffset { get; private set; }
        public int NameLength { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public ParameterHeader(ParameterType type, byte[] buffer, int offset) {
            Type = type;
            Id = BitConverter.ToInt32(buffer, offset + 0x00);
            NameOffset = BitConverter.ToInt32(buffer, offset + 0x04);
            NameLength = BitConverter.ToInt32(buffer, offset + 0x08);
        }

        #endregion

        #endregion
    }
}
