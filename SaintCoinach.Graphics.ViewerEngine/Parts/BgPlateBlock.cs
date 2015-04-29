using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace SaintCoinach.Graphics.Parts {
    public class BgPlateBlock : ComponentContainer {
        #region Fields
        private Point _MapPosition;
        private Vector3 _WorldPosition;
        private Matrix _Transformation;
        #endregion

        #region Properties
        public Point MapPosition { get { return _MapPosition; } }
        public Vector3 WorldPosition { get { return _WorldPosition; } }
        #endregion

        #region Constructor
        public BgPlateBlock(Assets.ModelFile file, Point mapPosition, Vector3 worldPosition) {
            _MapPosition = mapPosition;
            _WorldPosition = worldPosition;

            var model = file.GetModel();
            var subModel = model.GetSubModel(0);

            _Transformation = Matrix.Translation(worldPosition);

            foreach (var mesh in subModel.Meshes) {
                try {
                    Add(new Mesh(mesh));
                } catch { }
            }
        }
        #endregion

        public override void Draw(SharpDX.Direct3D11.Device device, EngineTime time, ref Matrix world, ref Matrix view, ref Matrix projection) {
            var transformedWorld = _Transformation * world;
            base.Draw(device, time, ref transformedWorld, ref view, ref projection);
        }
    }
}
