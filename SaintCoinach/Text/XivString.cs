using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    public class XivString : IStringNode, INodeWithChildren {
        private IStringNode[] _Children;

        TagType IStringNode.Tag { get { return TagType.None; } }
        NodeType IStringNode.Type { get { return NodeType.XivString; } }
        NodeFlags IStringNode.Flags { get { return NodeFlags.HasChildren | NodeFlags.IsExpression; } }
        public IEnumerable<IStringNode> Children { get { return _Children; } }

        public XivString(params IStringNode[] children) : this((IEnumerable<IStringNode>)children) { }
        public XivString(IEnumerable<IStringNode> children) {
            _Children = children.ToArray();
        }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder builder) {
            foreach (var part in Children)
                part.ToString(builder);
        }
    }
}
