using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SaintCoinach.Graphics.ShPk {
    public class ShaderHeader {
        #region Properties

        public int Size { get; private set; }
        public ShaderType Type { get; private set; }
        public int DataOffset { get; private set; }
        public int DataLength { get; private set; }
        public IReadOnlyList<ShaderParameterReference> ParameterReferences { get; private set; }

        #endregion

        #region Constructors

        public ShaderHeader(ShaderType type, byte[] buffer, int offset) {
            Size = 0x10;

            Type = type;

            DataOffset = BitConverter.ToInt32(buffer, offset + 0x00);
            DataLength = BitConverter.ToInt32(buffer, offset + 0x04);

            var cScalar = BitConverter.ToInt16(buffer, offset + 0x08);
            var cResource = BitConverter.ToInt16(buffer, offset + 0x0A);

            var paramRef = new List<ShaderParameterReference>();
            while (cScalar-- > 0) {
                paramRef.Add(new ShaderParameterReference(ParameterType.Scalar, buffer, offset + Size));
                Size += ShaderParameterReference.Size;
            }
            while (cResource-- > 0) {
                paramRef.Add(new ShaderParameterReference(ParameterType.Resource, buffer, offset + Size));
                Size += ShaderParameterReference.Size;
            }

            ParameterReferences = new ReadOnlyCollection<ShaderParameterReference>(paramRef);
        }

        #endregion
    }
}
