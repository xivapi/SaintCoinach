using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class CloseTag : IStringNode {
        private readonly TagType _Tag;

        public TagType Tag { get { return _Tag; } }
        NodeType IStringNode.Type { get { return NodeType.CloseTag; } }
        NodeFlags IStringNode.Flags { get { return NodeFlags.CloseTag; } }

        public CloseTag(TagType tag) {
            _Tag = tag;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder builder) {
            builder.Append(StringTokens.TagOpen);
            builder.Append(StringTokens.ElementClose);
            builder.Append(Tag);
            builder.Append(StringTokens.TagClose);
        }
    }
}
