using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class GenericElement : IStringNode, INodeWithChildren, INodeWithArguments {
        private TagType _Tag;
        private ArgumentCollection _Arguments;
        private IStringNode _Content;

        public TagType Tag { get { return _Tag; } }
        public IEnumerable<IStringNode> Arguments { get { return _Arguments; } }
        public IStringNode Content { get { return _Content; } }
        NodeType IStringNode.Type { get { return NodeType.GenericElement; } }
        NodeFlags IStringNode.Flags {
            get {
                var f = NodeFlags.OpenTag | NodeFlags.CloseTag | NodeFlags.IsExpression;
                if (_Arguments.HasItems)
                    f |= NodeFlags.HasArguments;
                if (Content != null)
                    f |= NodeFlags.HasChildren;
                return f;
            }
        }

        public GenericElement(TagType tag, IStringNode content, params IStringNode[] arguments) : this(tag, content, (IEnumerable<IStringNode>)arguments) { }
        public GenericElement(TagType tag, IStringNode content, IEnumerable<IStringNode> arguments) {
            _Tag = tag;
            _Arguments = new ArgumentCollection(arguments);
            _Content = content;
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

            if (Content == null) {
                builder.Append(StringTokens.ElementClose);
                builder.Append(StringTokens.TagClose);
            } else {
                builder.Append(StringTokens.TagClose);

                Content.ToString(builder);

                builder.Append(StringTokens.TagOpen);
                builder.Append(StringTokens.ElementClose);
                builder.Append(Tag);
                builder.Append(StringTokens.TagClose);
            }
        }

        #region INodeWithChildren Members

        IEnumerable<IStringNode> INodeWithChildren.Children {
            get { yield return Content; }
        }

        #endregion
    }
}
