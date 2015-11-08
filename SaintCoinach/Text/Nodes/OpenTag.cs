using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class OpenTag : INode, IExpressionNode {
        private readonly TagType _Tag;
        private readonly ArgumentCollection _Arguments;

        public TagType Tag { get { return _Tag; } }
        NodeFlags INode.Flags { get { return NodeFlags.IsExpression; } }
        public IEnumerable<INode> Arguments { get { return _Arguments; } }

        public OpenTag(TagType tag, params INode[] arguments) : this(tag, (IEnumerable<INode>)arguments) { }
        public OpenTag(TagType tag, IEnumerable<INode> arguments) {
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

        public T Accept<T>(SaintCoinach.Text.Nodes.INodeVisitor<T> visitor) {
            return visitor.Visit(this);
        }

        #region IExpressionNode Members

        public IExpression Evaluate(EvaluationParameters parameters) {
            return new Expressions.OpenTag(Tag, Arguments.Select(_ => _.TryEvaluate(parameters)));
        }

        #endregion
    }
}
