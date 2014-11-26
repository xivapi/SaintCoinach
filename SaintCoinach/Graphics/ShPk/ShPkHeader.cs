using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.ShPk {
    public class ShPkHeader {
        public const int Size = 0x48;

        #region Fields
        private int _FileLength;
        private int _ShaderDataOffset;
        private int _ParameterListOffset;
        private int _VertexShaderCount;
        private int _PixelShaderCount;

        private int _ScalarParameterCount;
        private int _ResourceParameterCount;

        private IReadOnlyList<ShaderHeader> _ShaderHeaders;
        private IReadOnlyList<ParameterHeader> _ParameterHeaders;
        #endregion

        #region Properties
        public int FileLength { get { return _FileLength; } }
        public int ShaderDataOffset { get { return _ShaderDataOffset; } }
        public int ParameterListOffset { get { return _ParameterListOffset; } }
        public int VertexShaderCount { get { return _VertexShaderCount; } }
        public int PixelShaderCount { get { return _PixelShaderCount; } }

        public int ScalarParameterCount { get { return _ScalarParameterCount; } }
        public int ResourceParameterCount { get { return _ResourceParameterCount; } }

        public IReadOnlyList<ShaderHeader> ShaderHeaders { get { return _ShaderHeaders; } }
        public IReadOnlyList<ParameterHeader> ParameterHeaders { get { return _ParameterHeaders; } }
        #endregion

        #region Constructor
        public ShPkHeader(byte[] buffer) {
            const uint Magic = 0x6B506853;
            const uint SupportedFormat = 0x00395844;

            if (BitConverter.ToUInt32(buffer, 0x00) != Magic)
                throw new NotSupportedException("File is not a ShPk.");
            if (BitConverter.ToUInt32(buffer, 0x08) != SupportedFormat)
                throw new NotSupportedException("Shader format is not supported.");

            _FileLength = BitConverter.ToInt32(buffer, 0x0C);
            _ShaderDataOffset = BitConverter.ToInt32(buffer, 0x10);
            _ParameterListOffset = BitConverter.ToInt32(buffer, 0x14);
            _VertexShaderCount = BitConverter.ToInt32(buffer, 0x18);
            _PixelShaderCount = BitConverter.ToInt32(buffer, 0x1C);

            _ScalarParameterCount = BitConverter.ToInt32(buffer, 0x28);
            _ResourceParameterCount = BitConverter.ToInt32(buffer, 0x2C);

            var offset = Size;
            var shs = new List<ShaderHeader>();
            for (var i = 0; i < VertexShaderCount; ++i) {
                var sh = new ShaderHeader(ShaderType.Vertex, buffer, offset);
                offset += sh.Size;
                shs.Add(sh);
            }
            for (var i = 0; i < PixelShaderCount; ++i) {
                var sh = new ShaderHeader(ShaderType.Pixel, buffer, offset);
                offset += sh.Size;
                shs.Add(sh);
            }
            _ShaderHeaders = new ReadOnlyCollection<ShaderHeader>(shs);


            var c1 = BitConverter.ToInt32(buffer, 0x24);
            offset += c1 * 0x08;

            var para = new List<ParameterHeader>();
            for (var i = 0; i < ScalarParameterCount; ++i) {
                var p = new ParameterHeader(ParameterType.Scalar, buffer, offset);
                offset += ParameterHeader.Size;
                para.Add(p);
            }
            for (var i = 0; i < ResourceParameterCount; ++i) {
                var p = new ParameterHeader(ParameterType.Resource, buffer, offset);
                offset += ParameterHeader.Size;
                para.Add(p);
            }
            _ParameterHeaders = new ReadOnlyCollection<ParameterHeader>(para);
        }
        #endregion
    }
}
