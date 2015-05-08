using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Device = SharpDX.Direct3D11.Device;

namespace SaintCoinach.Graphics.Viewer.Content {
    using SharpDX;

    public class PrimitiveModel : IDisposable {
        #region Properties
        public Model BaseModel { get; private set; }
        public PrimitiveMesh[] Meshes { get; private set; }
        #endregion

        #region Constructor
        public PrimitiveModel(Device device, Model baseModel) {
            this.BaseModel = baseModel;
            this.Meshes = BaseModel.Meshes.Select(m => new PrimitiveMesh(device, m)).ToArray();
        }
        #endregion

        #region IDisposable Members

        public void Dispose() {
            if (Meshes != null) {
                foreach (var m in Meshes)
                    m.Dispose();
            }
            Meshes = null;
        }

        #endregion
    }
}
