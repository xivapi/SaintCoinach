using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class Comparison : IStringNode {
        private readonly IStringNode _Left;
        private readonly ExpressionType _ComparisonType;
        private readonly IStringNode _Right;

        TagType IStringNode.Tag { get { return TagType.None; } }
        NodeType IStringNode.Type { get { return NodeType.Comparison; } }
        NodeFlags IStringNode.Flags { get { return NodeFlags.IsExpression; } }
        public IStringNode Left { get { return _Left; } }
        public ExpressionType ComparisonType { get { return _ComparisonType; } }
        public IStringNode Right { get { return _Right; } }

        public Comparison(ExpressionType comparisonType, IStringNode left, IStringNode right) {
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
    }
}
