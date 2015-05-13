using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Content {
    using Device = SharpDX.Direct3D11.Device;
    public class ContentMeshPart {
        #region Properties
        public ContentMesh Mesh { get; private set; }
        public MeshPart BasePart { get; private set; }
        #endregion

        #region Constructor
        public ContentMeshPart(ContentMesh mesh, MeshPart basePart) {
            this.Mesh = mesh;
            this.BasePart = basePart;
        }
        #endregion

        #region IDrawable3DComponent Members

        private bool? _VisibilityOverride = null;
        public bool? VisibilityOverride { get { return _VisibilityOverride; } set { _VisibilityOverride = value; } }

        public void Draw(Device device) {
            uint fullMask = 0;
            foreach (var attr in BasePart.Attributes)
                fullMask |= attr.AttributeMask;

            //var fromMask = (this.Mesh.Variant.ImcVariant.PartVisibilityMask & BasePart.Header.VisibilityMask) == BasePart.Header.VisibilityMask;
            var fromMask = (this.Mesh.Variant.ImcVariant.PartVisibilityMask & fullMask) == fullMask;
            if (VisibilityOverride.GetValueOrDefault(fromMask))
                device.ImmediateContext.DrawIndexed(BasePart.IndexCount, BasePart.IndexOffset - Mesh.Primitive.BaseMesh.Header.IndexBufferOffset, 0);
        }

        #endregion
    }
}
