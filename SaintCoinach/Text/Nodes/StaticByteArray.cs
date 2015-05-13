using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class StaticByteArray : IStringNode {
        private readonly byte[] _Value;

        TagType IStringNode.Tag { get { return TagType.None; } }
        NodeType IStringNode.Type { get { return NodeType.StaticValue; } }
        NodeFlags IStringNode.Flags { get { return NodeFlags.IsExpression; } }
        public byte[] Value { get { return _Value; } }

        public StaticByteArray(byte[] value) {
            _Value = value.ToArray();
        }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder builder) {
            for (var i = 0; i < Value.Length; ++i)
                builder.Append(Value[i].ToString("X2"));
        }
    }
}
