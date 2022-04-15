using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class IfElement : INode, IConditionalNode {
        private readonly TagType _Tag;
        private readonly INode _Condition;
        private readonly INode _TrueValue;
        private readonly INode _FalseValue;
        private readonly String _LenByte;

        public TagType Tag { get { return _Tag; } }
        NodeFlags INode.Flags { get { return NodeFlags.IsExpression | NodeFlags.IsConditional; } }
        public INode Condition { get { return _Condition; } }
        public INode TrueValue { get { return _TrueValue; } }
        public INode FalseValue { get { return _FalseValue; } }
        public String LenByte { get { return _LenByte; } }

        public IfElement(TagType tag, INode condition, INode trueValue, INode falseValue, String lenByte) {
            if (condition == null)
                throw new ArgumentNullException("condition");
            _Tag = tag;
            _Condition = condition;
            _TrueValue = trueValue;
            _FalseValue = falseValue;
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
            Condition.ToString(builder);

            if (TrueValue != null)
                TrueValue.ToString(builder);

            if (FalseValue != null) {
                FalseValue.ToString(builder);
            }

            builder.Append("03");
            builder.Append(StringTokens.TagClose);
        }

        public T Accept<T>(SaintCoinach.Text.Nodes.INodeVisitor<T> visitor) {
            return visitor.Visit(this);
        }

        #region IExpressionNode Members

        public IExpression Evaluate(EvaluationParameters parameters) {
            var evalCond = Condition.TryEvaluate(parameters);
            if (parameters.FunctionProvider.ToBoolean(evalCond))
                return TrueValue.TryEvaluate(parameters);
            return FalseValue.TryEvaluate(parameters);
        }

        #endregion
    }
}
