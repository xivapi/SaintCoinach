using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class SwitchElement : IStringNode {
        private readonly TagType _Tag;
        private readonly IStringNode _CaseSwitch;
        private readonly ReadOnlyDictionary<int, IStringNode> _Cases;

        public TagType Tag { get { return _Tag; } }
        NodeType IStringNode.Type { get { return NodeType.Switch; } }
        NodeFlags IStringNode.Flags { get { return NodeFlags.OpenTag | NodeFlags.CloseTag | NodeFlags.IsExpression; } }
        public IStringNode CaseSwitch { get { return _CaseSwitch; } }
        public IReadOnlyDictionary<int, IStringNode> Cases { get { return _Cases; } }

        public SwitchElement(TagType tag, IStringNode caseSwitch, IDictionary<int, IStringNode> cases) {
            if (caseSwitch == null)
                throw new ArgumentNullException("caseSwitch");
            if (cases == null)
                throw new ArgumentNullException("cases");
            _Tag = tag;
            _CaseSwitch = caseSwitch;
            _Cases = new ReadOnlyDictionary<int, IStringNode>(cases);
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
            CaseSwitch.ToString(builder);
            builder.Append(StringTokens.ArgumentsClose);
            builder.Append(StringTokens.TagClose);

            foreach(var caseValue in Cases){
                builder.Append(StringTokens.TagOpen);
                builder.Append(StringTokens.CaseTagName);
                builder.Append(StringTokens.ArgumentsOpen);
                builder.Append(caseValue.Key);
                builder.Append(StringTokens.ArgumentsClose);
                builder.Append(StringTokens.TagClose);

                caseValue.Value.ToString(builder);

                builder.Append(StringTokens.TagOpen);
                builder.Append(StringTokens.ElementClose);
                builder.Append(StringTokens.CaseTagName);
                builder.Append(StringTokens.TagClose);
            }

            builder.Append(StringTokens.TagOpen);
            builder.Append(StringTokens.ElementClose);
            builder.Append(Tag);
            builder.Append(StringTokens.TagClose);
        }
    }
}
