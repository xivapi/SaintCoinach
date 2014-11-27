using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace SaintCoinach.Graphics {
    public interface IGeometricPrimitive : IDisposable {
        Buffer VertexBuffer { get; }
        VertexBufferBinding VertexBinding { get; }
        Buffer IndexBuffer { get; }

        Type VertexType { get; }
    }
}
