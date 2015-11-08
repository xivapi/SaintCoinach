using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class StaticInteger : IStaticNode {
        private readonly int _Value;

        TagType INode.Tag { get { return TagType.None; } }
        NodeFlags INode.Flags { get { return NodeFlags.IsStatic; } }
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
