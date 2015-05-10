using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SaintCoinach.Graphics.Viewer.Content {
    using SharpDX;
    using SharpDX.Direct3D11;
    using Device = SharpDX.Direct3D11.Device;
    using Buffer = SharpDX.Direct3D11.Buffer;

    public class PrimitiveMesh : IDisposable {
        #region Properties
        public Mesh BaseMesh { get; private set; }
        public Buffer IndexBuffer { get; private set; }
        public Buffer VertexBuffer { get; private set; }
        public VertexBufferBinding VertexBufferBinding { get; private set; }
        #endregion

        #region Constructor
        public PrimitiveMesh(Device device, Mesh baseMesh) {
            BaseMesh = baseMesh;

            var vert3d = new Vertex3D[baseMesh.Vertices.Length];
            for (var i = 0; i < vert3d.Length; ++i)
                vert3d[i] = new Vertex3D(baseMesh.Vertices[i]);

            IndexBuffer = Buffer.Create<ushort>(device, BindFlags.IndexBuffer, baseMesh.Indices);
            VertexBuffer = Buffer.Create<Vertex3D>(device, BindFlags.VertexBuffer, vert3d);
            VertexBufferBinding = new VertexBufferBinding(VertexBuffer, VertexBuffer.Description.SizeInBytes / baseMesh.Vertices.Length, 0);
        }
        #endregion

        #region IDisposable Members

        public void Dispose() {
            if (IndexBuffer != null)
                IndexBuffer.Dispose();
            IndexBuffer = null;
            if (VertexBuffer != null)
                VertexBuffer.Dispose();
            VertexBuffer = null;
        }

        #endregion
    }
}
