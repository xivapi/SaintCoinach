using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class StaticString : IStringNode {
        private readonly string _Value;

        TagType IStringNode.Tag { get { return TagType.None; } }
        NodeType IStringNode.Type { get { return NodeType.StaticValue; } }
        NodeFlags IStringNode.Flags { get { return NodeFlags.IsExpression; } }
        public string Value { get { return _Value; } }

        public StaticString(string value) {
            _Value = value;
        }

        public override string ToString() {
            return Value;
        }
        public void ToString(StringBuilder builder) {
            builder.Append(Value);
        }
    }
}
