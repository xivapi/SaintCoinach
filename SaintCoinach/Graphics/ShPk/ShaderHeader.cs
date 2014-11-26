using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.ShPk {
    public class ShaderHeader {
        #region Fields
        private int _Size;
        private ShaderType _Type;
        private int _DataOffset;
        private int _DataLength;

        private IReadOnlyList<ShaderParameterReference> _ParameterReferences;
        #endregion

        #region Properties
        public int Size { get { return _Size; } }
        public ShaderType Type { get { return _Type; } }
        public int DataOffset { get { return _DataOffset; } }
        public int DataLength { get { return _DataLength; } }

        public IReadOnlyList<ShaderParameterReference> ParameterReferences { get { return _ParameterReferences; } }
        #endregion

        #region Constructor
        public ShaderHeader(ShaderType type, byte[] buffer, int offset) {
            _Size = 0x10;

            _DataOffset = BitConverter.ToInt32(buffer, offset + 0x00);
            _DataLength = BitConverter.ToInt32(buffer, offset + 0x04);

            var cScalar = BitConverter.ToInt16(buffer, offset + 0x08);
            var cResource = BitConverter.ToInt16(buffer, offset + 0x0A);

            var paramRef = new List<ShaderParameterReference>();
            while(cScalar-- > 0) {
                paramRef.Add(new ShaderParameterReference(ParameterType.Scalar, buffer, offset + _Size));
                _Size += ShaderParameterReference.Size;
            }
            while (cResource-- > 0) {
                paramRef.Add(new ShaderParameterReference(ParameterType.Resource, buffer, offset + _Size));
                _Size += ShaderParameterReference.Size;
            }

            _ParameterReferences = new ReadOnlyCollection<ShaderParameterReference>(paramRef);
        }
        #endregion
    }
}
