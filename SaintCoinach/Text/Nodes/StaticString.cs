using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class StaticString : IStaticNode {
        private readonly string _Value;

        TagType INode.Tag { get { return TagType.None; } }
        NodeType INode.Type { get { return NodeType.StaticValue; } }
        NodeFlags INode.Flags { get { return NodeFlags.IsStatic; } }
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

        #region IDecodeNodeStatic Members

        object IStaticNode.Value {
            get { return Value; }
        }

        #endregion
    }
}
