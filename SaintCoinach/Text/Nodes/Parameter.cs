using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class Parameter : INode, IExpressionNode {
        private readonly DecodeExpressionType _ParameterType;
        private readonly INode _ParameterIndex;

        TagType INode.Tag { get { return TagType.None; } }
        NodeFlags INode.Flags { get { return NodeFlags.IsExpression; } }
        public DecodeExpressionType ParameterType { get { return _ParameterType; } }
        public INode ParameterIndex { get { return _ParameterIndex; } }

        public Parameter(DecodeExpressionType parameterType, INode parameterIndex) {
            if (parameterIndex == null)
                throw new ArgumentNullException("parameterIndex");
            _ParameterType = parameterType;
            _ParameterIndex = parameterIndex;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder builder) {
            builder.Append(ParameterType);
            builder.Append(StringTokens.ArgumentsOpen);
            ParameterIndex.ToString(builder);
            builder.Append(StringTokens.ArgumentsClose);
        }

        public T Accept<T>(SaintCoinach.Text.Nodes.INodeVisitor<T> visitor) {
            return visitor.Visit(this);
        }

        #region IExpressionNode Members

        public IExpression Evaluate(EvaluationParameters parameters) {
            var evalIndex = ParameterIndex.TryEvaluate(parameters);
            var index = parameters.FunctionProvider.ToInteger(evalIndex);
            return new Expressions.GenericExpression(parameters[this.ParameterType, index]);
        }

        #endregion
    }
}
