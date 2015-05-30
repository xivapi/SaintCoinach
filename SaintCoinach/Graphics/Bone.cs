using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [System.Diagnostics.DebuggerDisplay("{Name} ( {Unknown1.X}, {Unknown1.Y}, {Unknown1.Z}, {Unknown1.W} ) -> ( {Unknown2.X}, {Unknown2.Y}, {Unknown2.Z}, {Unknown2.W} )")]
    public class Bone {
        #region Properties
        public ModelDefinition Definition { get; private set; }
        public string Name { get; private set; }
        public int Index { get; private set; }
        public Vector4 Unknown1 { get; private set; }
        public Vector4 Unknown2 { get; private set; }
        #endregion

        #region Constructor
        internal Bone(ModelDefinition definition, int index, byte[] buffer, ref int offset) {
            this.Definition = definition;
            this.Index = index;
            this.Name = definition.BoneNames[index];

            this.Unknown1 = buffer.ToStructure<Vector4>(ref offset);
            this.Unknown2 = buffer.ToStructure<Vector4>(ref offset);
        }
        #endregion
    }
}
