using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class OpenTag : IStringNode {
        private readonly TagType _Tag;
        private readonly ArgumentCollection _Arguments;

        public TagType Tag { get { return _Tag; } }
        NodeType IStringNode.Type { get { return NodeType.OpenTag; } }
        NodeFlags IStringNode.Flags { get { return NodeFlags.CloseTag; } }
        public IEnumerable<IStringNode> Arguments { get { return _Arguments; } }

        public OpenTag(TagType tag, params IStringNode[] arguments) : this(tag, (IEnumerable<IStringNode>)arguments) { }
        public OpenTag(TagType tag, IEnumerable<IStringNode> arguments) {
            _Tag = tag;
            _Arguments = new ArgumentCollection(arguments);
        }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder builder) {
            builder.Append(StringTokens.TagOpen);
            builder.Append(Tag);

            _Arguments.ToString(builder);

            builder.Append(StringTokens.TagClose);
        }
    }
}
