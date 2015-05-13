using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class StaticInteger : IStringNode {
        private readonly int _Value;

        TagType IStringNode.Tag { get { return TagType.None; } }
        NodeType IStringNode.Type { get { return NodeType.StaticValue; } }
        NodeFlags IStringNode.Flags { get { return NodeFlags.IsExpression; } }
        public int Value { get { return _Value; } }

        public StaticInteger(int value) {
            _Value = value;
        }

        public override string ToString() {
            return Value.ToString();
        }
        public void ToString(StringBuilder builder) {
            builder.Append(Value);
        }
    }
}
