using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics.Primitives {
    using Buffer = SharpDX.Direct3D11.Buffer;

    public class GeometricPrimitive<T> : IGeometricPrimitive where T : struct {
        #region Fields
        private Device _Device;
        private Buffer _IndexBuffer;
        private Buffer _VertexBuffer;

        private VertexBufferBinding _VertexBinding;
        #endregion

        #region Properties
        public Buffer IndexBuffer { get { return _IndexBuffer; } }
        public Buffer VertexBuffer { get { return _VertexBuffer; } }
        public VertexBufferBinding VertexBinding { get { return _VertexBinding; } }
        public Type VertexType { get { return typeof(T); } }
        #endregion

        #region Constructor
        public GeometricPrimitive(Device device, T[] vertices, int[] indices)
            : this(device, vertices, indices.Select(_ => (ushort)_).ToArray()) {
        }
        public GeometricPrimitive(Device device, T[] vertices, short[] indices)
            : this(device, vertices, indices.Select(_ => (ushort)_).ToArray()) {
        }
        public GeometricPrimitive(Device device, T[] vertices, ushort[] indices) {
            _Device = device;

            /*if (indices.Length >= 0xFFFF)
                throw new InvalidOperationException("Cannot generate more than 65535 indices.");*/

            _VertexBuffer = Buffer.Create<T>(device, BindFlags.VertexBuffer, vertices);
            _IndexBuffer = Buffer.Create<ushort>(device, BindFlags.IndexBuffer, indices);
            _VertexBinding = new VertexBufferBinding(_VertexBuffer, _VertexBuffer.Description.SizeInBytes / vertices.Length, 0);
        }
        #endregion

        #region Reverse winding
        static void ReverseWinding(T[] vertices, ushort[] indices) {
            for (var i = 0; i < indices.Length; ++i) {
                var swap = indices[i];
                indices[i] = indices[i + 2];
                indices[i + 2] = swap;
            }
        }
        #endregion


        #region IDisposable Members

        public void Dispose() {
            if (_VertexBuffer != null)
                _VertexBuffer.Dispose();
            _VertexBuffer = null;

            if (_IndexBuffer != null)
                _IndexBuffer.Dispose();
            _IndexBuffer = null;
        }

        #endregion
    }
}
