using System;

namespace SaintCoinach.Graphics.Assets {
    public class MaterialParameterMapping {
        #region Static

        public const int Size = 0x0C;

        #endregion

        #region Properties

        public int Index { get; private set; }
        public uint Id { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public MaterialParameterMapping(byte[] buffer, int offset) {
            Id = BitConverter.ToUInt32(buffer, offset + 0x00);
            Index = BitConverter.ToInt32(buffer, offset + 0x08);
        }

        #endregion

        #endregion
    }
}
