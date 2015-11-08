using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class Comparison : INode, IExpressionNode {
        private readonly INode _Left;
        private readonly DecodeExpressionType _ComparisonType;
        private readonly INode _Right;

        TagType INode.Tag { get { return TagType.None; } }
        NodeFlags INode.Flags { get { return NodeFlags.IsExpression; } }
        public INode Left { get { return _Left; } }
        public DecodeExpressionType ComparisonType { get { return _ComparisonType; } }
        public INode Right { get { return _Right; } }

        public Comparison(DecodeExpressionType comparisonType, INode left, INode right) {
            _ComparisonType = comparisonType;
            _Left = left;
            _Right = right;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder builder) {
            builder.Append(ComparisonType);
            builder.Append(StringTokens.ArgumentsOpen);
            if (Left != null)
                Left.ToString(builder);
            builder.Append(StringTokens.ArgumentsSeperator);
            if (Right != null)
                Right.ToString(builder);
            builder.Append(StringTokens.ArgumentsClose);
        }

        public T Accept<T>(SaintCoinach.Text.Nodes.INodeVisitor<T> visitor) {
            return visitor.Visit(this);
        }

        #region IExpressionNode Members

        public IExpression Evaluate(EvaluationParameters parameters) {
            return new Expressions.GenericExpression(parameters.FunctionProvider.Compare(parameters, ComparisonType, Left, Right));
        }

        #endregion
    }
}
