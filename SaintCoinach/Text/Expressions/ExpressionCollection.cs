using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Expressions {
    public class ExpressionCollection : IExpression {
        private readonly IExpression[] _Children;

        public IEnumerable<IExpression> Children { get { return _Children; } }
        public string Separator { get; set; }

        public ExpressionCollection(params IExpression[] children) : this((IEnumerable<IExpression>)children) { }
        public ExpressionCollection(IEnumerable<IExpression> children) {
            if (children == null)
                _Children = new IExpression[0];
            else
                _Children = children.ToArray();
        }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder output) {
            var addSeparator = false;
            foreach (var c in Children) {
                if (addSeparator)
                    output.Append(Separator);
                else
                    addSeparator = true;

                if (c != null)
                    c.ToString(output);
            }
        }
    }
}
