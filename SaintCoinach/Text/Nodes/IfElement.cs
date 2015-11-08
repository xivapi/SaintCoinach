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

        public TagType Tag { get { return _Tag; } }
        NodeFlags INode.Flags { get { return NodeFlags.IsExpression | NodeFlags.IsConditional; } }
        public INode Condition { get { return _Condition; } }
        public INode TrueValue { get { return _TrueValue; } }
        public INode FalseValue { get { return _FalseValue; } }

        public IfElement(TagType tag, INode condition, INode trueValue, INode falseValue) {
            if (condition == null)
                throw new ArgumentNullException("condition");
            _Tag = tag;
            _Condition = condition;
            _TrueValue = trueValue;
            _FalseValue = falseValue;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder builder) {
            builder.Append(StringTokens.TagOpen);
            builder.Append(Tag);
            builder.Append(StringTokens.ArgumentsOpen);
            Condition.ToString(builder);
            builder.Append(StringTokens.ArgumentsClose);
            builder.Append(StringTokens.TagClose);

            if (TrueValue != null)
                TrueValue.ToString(builder);

            if (FalseValue != null) {
                builder.Append(StringTokens.ElseTag);
                FalseValue.ToString(builder);
            }

            builder.Append(StringTokens.TagOpen);
            builder.Append(StringTokens.ElementClose);
            builder.Append(Tag);
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
