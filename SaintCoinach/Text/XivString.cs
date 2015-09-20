using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    using Nodes;

    public class XivString : INode, IExpressionNode, INodeWithChildren, IComparable, IComparable<XivString>, IComparable<string>, IEquatable<XivString>, IEquatable<string> {
        public static readonly XivString Empty = new XivString(new INode[0]);

        private readonly INode[] _Children;
        private WeakReference<string> _StringCache;

        TagType INode.Tag { get { return TagType.None; } }
        NodeFlags INode.Flags { get { return NodeFlags.HasChildren | NodeFlags.IsExpression; } }
        public IEnumerable<INode> Children { get { return _Children; } }

        public bool IsEmpty {
            get {
                if (_Children.Length == 0)
                    return true;
                // TODO: This can be made more efficient (as in, without the need to convert to a string), but that's going to take lots of work.
                return string.IsNullOrWhiteSpace(ToString());
            }
        }

        public XivString(params INode[] children) : this((IEnumerable<INode>)children) { }
        public XivString(IEnumerable<INode> children) {
            _Children = children.ToArray();
        }

        public override string ToString() {
            string v;
            if (_StringCache != null && _StringCache.TryGetTarget(out v))
                return v;
            var sb = new StringBuilder();
            ToString(sb);
            v = sb.ToString();
            if (_StringCache == null)
                _StringCache = new WeakReference<string>(v);
            else
                _StringCache.SetTarget(v);
            return v;
        }
        public void ToString(StringBuilder builder) {
            foreach (var part in Children)
                part.ToString(builder);
        }

        public static implicit operator string (XivString xivString) {
            return xivString.ToString();
        }

        public override int GetHashCode() {
            return ToString().GetHashCode();
        }
        public override bool Equals(object obj) {
            if (obj is XivString)
                return string.Equals(this.ToString(), obj.ToString());
            if (obj is string)
                return string.Equals(this.ToString(), (string)obj);
            return false;
        }
        public static bool operator ==(XivString left, XivString right) {
            var lNull = object.ReferenceEquals(null, left);
            var rNull = object.ReferenceEquals(null, right);
            if (lNull && rNull)
                return true;
            if (lNull != rNull)
                return false;
            return (left.ToString() == right.ToString());
        }
        public static bool operator !=(XivString left, XivString right) {
            var lNull = object.ReferenceEquals(null, left);
            var rNull = object.ReferenceEquals(null, right);
            if (lNull && rNull)
                return false;
            if (lNull != rNull)
                return true;
            return (left.ToString() != right.ToString());
        }
        public static bool operator ==(XivString left, string right) {
            var lNull = object.ReferenceEquals(null, left);
            var rNull = object.ReferenceEquals(null, right);
            if (lNull && rNull)
                return true;
            if (lNull != rNull)
                return false;
            return (left.ToString() == right);
        }
        public static bool operator !=(XivString left, string right) {
            var lNull = object.ReferenceEquals(null, left);
            var rNull = object.ReferenceEquals(null, right);
            if (lNull && rNull)
                return false;
            if (lNull != rNull)
                return true;
            return (left.ToString() != right);
        }

        #region IComparable Members

        public int CompareTo(object obj) {
            if (obj is XivString)
                return CompareTo((XivString)obj);
            if (obj is string)
                return CompareTo((string)obj);
            if (obj == null)
                return 1;
            return -1;
        }

        #endregion

        #region IComparable<XivString> Members

        public int CompareTo(XivString other) {
            return string.Compare(this.ToString(), other.ToString());
        }

        #endregion

        #region IComparable<string> Members

        public int CompareTo(string other) {
            return string.Compare(this.ToString(), other);
        }

        #endregion

        #region IEquatable<XivString> Members

        public bool Equals(XivString other) {
            return string.Equals(this.ToString(), other.ToString());
        }

        #endregion

        #region IEquatable<string> Members

        public bool Equals(string other) {
            return string.Equals(this.ToString(), other);
        }

        #endregion

        #region IExpressionNode Members

        public IExpression Evaluate(EvaluationParameters parameters) {
            return new Expressions.ExpressionCollection(Children.Select(c => c.TryEvaluate(parameters)));
        }

        #endregion

        public T Accept<T>(SaintCoinach.Text.Nodes.INodeVisitor<T> visitor) {
            return visitor.Visit(this);
        }
    }
}
