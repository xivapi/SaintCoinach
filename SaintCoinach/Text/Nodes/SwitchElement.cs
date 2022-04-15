using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class SwitchElement : INode, IExpressionNode {
        private readonly TagType _Tag;
        private readonly INode _CaseSwitch;
        private readonly ReadOnlyDictionary<int, INode> _Cases;
        private readonly String _LenByte;

        public TagType Tag { get { return _Tag; } }
        NodeFlags INode.Flags { get { return NodeFlags.IsExpression; } }
        public INode CaseSwitch { get { return _CaseSwitch; } }
        public IReadOnlyDictionary<int, INode> Cases { get { return _Cases; } }
        public String LenByte { get { return _LenByte; } } 

        public SwitchElement(TagType tag, INode caseSwitch, IDictionary<int, INode> cases, String lenByte) {
            if (caseSwitch == null)
                throw new ArgumentNullException("caseSwitch");
            if (cases == null)
                throw new ArgumentNullException("cases");
            _Tag = tag;
            _CaseSwitch = caseSwitch;
            _Cases = new ReadOnlyDictionary<int, INode>(cases);
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
            CaseSwitch.ToString(builder);

            foreach (var caseValue in Cases) {
                caseValue.Value.ToString(builder);
            }

            builder.Append("03");
            builder.Append(StringTokens.TagClose);
        }

        public T Accept<T>(SaintCoinach.Text.Nodes.INodeVisitor<T> visitor) {
            return visitor.Visit(this);
        }

        #region IExpressionNode Members

        public IExpression Evaluate(EvaluationParameters parameters) {
            var evalSwitch = CaseSwitch.TryEvaluate(parameters);
            var asInt = parameters.FunctionProvider.ToInteger(evalSwitch);

            INode caseNode;
            if (!_Cases.TryGetValue(asInt, out caseNode))
                throw new InvalidOperationException();
            return caseNode.TryEvaluate(parameters);
        }

        #endregion
    }
}
