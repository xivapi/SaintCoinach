using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class StaticString : IStaticNode {
        private readonly string _Value;

        TagType INode.Tag { get { return TagType.None; } }
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

        public T Accept<T>(SaintCoinach.Text.Nodes.INodeVisitor<T> visitor) {
            return visitor.Visit(this);
        }

        #region IDecodeNodeStatic Members

        object IStaticNode.Value {
            get { return Value; }
        }

        #endregion
    }
}
