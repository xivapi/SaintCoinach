using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class CloseTag : IExpressionNode {
        private readonly TagType _Tag;
        private readonly String _LenByte;

        public TagType Tag { get { return _Tag; } }
        NodeFlags INode.Flags { get { return NodeFlags.IsExpression | NodeFlags.IsStatic; } }
        public String LenByte { get { return _LenByte; } }

        public CloseTag(TagType tag, String lenByte) {
            _Tag = tag;
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
            builder.Append(((byte)Tag).ToString("X2"));
            builder.Append(LenByte);
            builder.Append("03");
            builder.Append(StringTokens.TagClose);
        }

        public T Accept<T>(SaintCoinach.Text.Nodes.INodeVisitor<T> visitor) {
            return visitor.Visit(this);
        }

        #region IExpressionNode Members

        public IExpression Evaluate(EvaluationParameters parameters) {
            return new Expressions.CloseTag(Tag);
        }

        #endregion
    }
}
