using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    public interface INode {
        TagType Tag { get; }
        NodeFlags Flags { get; }
        void ToString(StringBuilder builder);
        T Accept<T>(SaintCoinach.Text.Nodes.INodeVisitor<T> visitor);
    }
}
