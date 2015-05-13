using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class DefaultElement : IStringNode, INodeWithChildren {
        private readonly TagType _Tag;
        private readonly StaticByteArray _Data;

        public TagType Tag { get { return _Tag; } }
        public IStringNode Data { get { return _Data; } }
        NodeType IStringNode.Type { get { return NodeType.DefaultElement; } }
        NodeFlags IStringNode.Flags { get { return NodeFlags.OpenTag | NodeFlags.HasChildren | NodeFlags.CloseTag | NodeFlags.IsExpression; } }

        public DefaultElement(TagType tag, byte[] innerBuffer) {
            _Tag = tag;
            _Data = new StaticByteArray(innerBuffer);
        }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder builder) {
            builder.Append(StringTokens.TagOpen);
            builder.Append(Tag);
            if (_Data.Value.Length == 0) {
                builder.Append(StringTokens.ElementClose);
                builder.Append(StringTokens.TagClose);
            } else {
                builder.Append(StringTokens.TagClose);

                _Data.ToString(builder);

                builder.Append(StringTokens.TagOpen);
                builder.Append(StringTokens.ElementClose);
                builder.Append(Tag);
                builder.Append(StringTokens.TagClose);
            }
        }

        #region INodeWithChildren Members

        IEnumerable<IStringNode> INodeWithChildren.Children {
            get { yield return Data; }
        }

        #endregion
    }
}
