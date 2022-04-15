using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class StaticInteger : IStaticNode {
        private readonly int _Value;
        private readonly String _LenByte;

        TagType INode.Tag { get { return TagType.None; } }
        NodeFlags INode.Flags { get { return NodeFlags.IsStatic; } }
        public int Value { get { return _Value; } }
        public String LenByte { get { return _LenByte; } }

        public StaticInteger(int value, String lenByte) {
            _Value = value;
            _LenByte = lenByte;
        }

        public override string ToString() {
            return Value.ToString();
        }
        public void ToString(StringBuilder builder) {
            builder.Append(LenByte);
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
