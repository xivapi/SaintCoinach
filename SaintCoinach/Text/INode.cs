using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    public interface INode {
        TagType Tag { get; }
        NodeType Type { get; }
        NodeFlags Flags { get; }
        void ToString(StringBuilder builder);
    }
}
