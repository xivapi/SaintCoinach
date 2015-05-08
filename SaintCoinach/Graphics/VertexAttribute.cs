using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public enum VertexAttribute : byte {
        Position = 0x00,
        BlendWeights = 0x01,
        BlendIndices = 0x02,
        Normal = 0x03,
        UV = 0x04,
        Tangent2 = 0x05, // Don't ask me why the second one (only present on dual-textured models) is first
        Tangent1 = 0x06,
        Color = 0x07,

        // TODO: Check for additional types (chara/* models checked, most in bg/* too, but some couldn't be read)
    }
}
