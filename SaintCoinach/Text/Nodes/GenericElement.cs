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
        private String _LenByte;

        public TagType Tag { get { return _Tag; } }
        public IEnumerable<INode> Arguments { get { return _Arguments; } }
        public INode Content { get { return _Content; } }
        public String LenByte { get { return _LenByte;  } }
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

        public GenericElement(TagType tag, INode content, String lenByte, params INode[] arguments) : this(tag, content, lenByte, (IEnumerable<INode>)arguments) { }
        public GenericElement(TagType tag, INode content, String lenByte, IEnumerable<INode> arguments) {
            _Tag = tag;
            _Arguments = new ArgumentCollection(arguments);
            _Content = content;
            _LenByte = lenByte;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder builder) {
            builder.Append(StringTokens.TagOpen);
            builder.Append("hex:02");
            builder.Append(((byte)Tag).ToString("X2")); /* X means hex, 2 means 2-digit */
            builder.Append(LenByte);

            if (Content != null) {
                Content.ToString(builder);
            }

            _Arguments.ToString(builder);
            builder.Append("03");
            builder.Append(StringTokens.TagClose);
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
