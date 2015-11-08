using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class StaticByteArray : INode, IStaticNode {
        private readonly byte[] _Value;

        TagType INode.Tag { get { return TagType.None; } }
        NodeFlags INode.Flags { get { return NodeFlags.IsStatic; } }
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

        public T Accept<T>(SaintCoinach.Text.Nodes.INodeVisitor<T> visitor) {
            return visitor.Visit(this);
        }

        #region IDecodeNodeStatic Members

        object IStaticNode.Value {
            get { return ToString(); }
        }

        #endregion
    }
}
