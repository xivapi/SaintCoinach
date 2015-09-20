using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class GenericElement : INode, INodeWithChildren, INodeWithArguments, IExpressionNode {
        private TagType _Tag;
        private ArgumentCollection _Arguments;
        private INode _Content;

        public TagType Tag { get { return _Tag; } }
        public IEnumerable<INode> Arguments { get { return _Arguments; } }
        public INode Content { get { return _Content; } }
        NodeFlags INode.Flags {
            get {
                var f = NodeFlags.IsExpression;
                if (_Arguments.HasItems)
                    f |= NodeFlags.HasArguments;
                if (Content != null)
                    f |= NodeFlags.HasChildren;
                return f;
            }
        }

        public GenericElement(TagType tag, INode content, params INode[] arguments) : this(tag, content, (IEnumerable<INode>)arguments) { }
        public GenericElement(TagType tag, INode content, IEnumerable<INode> arguments) {
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

        public T Accept<T>(SaintCoinach.Text.Nodes.INodeVisitor<T> visitor) {
            return visitor.Visit(this);
        }

        #region INodeWithChildren Members

        IEnumerable<INode> INodeWithChildren.Children {
            get { yield return Content; }
        }

        #endregion

        #region IExpressionNode Members

        public IExpression Evaluate(EvaluationParameters parameters) {
            return parameters.FunctionProvider.EvaluateGenericElement(parameters, this);
        }

        #endregion
    }
}
