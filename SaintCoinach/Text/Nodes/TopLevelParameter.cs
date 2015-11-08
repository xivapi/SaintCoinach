using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class TopLevelParameter : INode, IExpressionNode {
        private readonly int _Value;

        public int Value { get { return _Value; } }

        public TopLevelParameter(int value) {
            _Value = value;
        }

        #region IExpressionNode Members

        public IExpression Evaluate(EvaluationParameters parameters) {
            return new Expressions.GenericExpression(parameters.TopLevelParameters[Value]);
        }

        #endregion

        #region INode Members


        TagType INode.Tag { get { return TagType.None; } }
        NodeFlags INode.Flags { get { return NodeFlags.IsExpression; } }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder builder) {
            builder.Append(StringTokens.TopLevelParameterName);
            builder.Append(StringTokens.ArgumentsOpen);
            builder.Append(Value);
            builder.Append(StringTokens.ArgumentsClose);
        }

        public T Accept<T>(SaintCoinach.Text.Nodes.INodeVisitor<T> visitor) {
            return visitor.Visit(this);
        }
        #endregion
    }
}
