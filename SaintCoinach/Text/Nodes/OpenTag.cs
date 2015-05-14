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
        NodeType INode.Type { get { return NodeType.OpenTag; } }
        NodeFlags INode.Flags { get { return _Arguments.HasItems ? NodeFlags.IsExpression : NodeFlags.IsStatic; } }
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

        #region IExpressionNode Members

        public IExpression Evaluate(EvaluationParameters parameters) {
            if (!_Arguments.HasItems)
                throw new InvalidOperationException();

            return new Expressions.SurroundedExpression(
                string.Format("{0}{1}{2}", StringTokens.TagOpen, Tag, StringTokens.ArgumentsOpen),
                new Expressions.ExpressionCollection(Arguments.Select(_ => _.TryEvaluate(parameters))),
                StringTokens.ArgumentsClose + StringTokens.TagClose);
        }

        #endregion
    }
}
