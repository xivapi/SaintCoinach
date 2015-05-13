using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class IfElement : IStringNode, IConditionalNode {
        private readonly TagType _Tag;
        private readonly IStringNode _Condition;
        private readonly IStringNode _TrueValue;
        private readonly IStringNode _FalseValue;

        public TagType Tag { get { return _Tag; } }
        NodeType IStringNode.Type { get { return NodeType.IfElement; } }
        NodeFlags IStringNode.Flags { get { return NodeFlags.OpenTag | NodeFlags.CloseTag | NodeFlags.IsExpression | NodeFlags.IsConditional; } }
        public IStringNode Condition { get { return _Condition; } }
        public IStringNode TrueValue { get { return _TrueValue; } }
        public IStringNode FalseValue { get { return _FalseValue; } }

        public IfElement(TagType tag, IStringNode condition, IStringNode trueValue, IStringNode falseValue) {
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
    }
}
