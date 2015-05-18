using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class TransformedModel {
        #region Properties
        public Vector3 Translation { get; private set; }
        public Vector3 Rotation { get; private set; }
        public Vector3 Scale { get; private set; }
        public ModelDefinition Model { get; private set; }
        #endregion

        #region Constructor
        public TransformedModel(ModelDefinition model, Vector3 translation, Vector3 rotation, Vector3 scale) {
            this.Model = model;
            this.Translation = translation;
            this.Rotation = rotation;
            this.Scale = scale;
        }
        #endregion
    }
}
