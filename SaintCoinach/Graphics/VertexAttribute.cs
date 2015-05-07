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
        Unknown06 = 0x06, // TODO: purpose unknown, possibly shinyness?
        Color = 0x07,

        // TODO: Check for additional attributes (chara/* models checked, but not bg/*)
    }
}
