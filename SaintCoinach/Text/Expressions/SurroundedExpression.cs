using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Expressions {
    public class SurroundedExpression : IValueExpression {
        private readonly object _Prefix;
        private readonly object _Value;
        private readonly object _Suffix;

        public object Prefix { get { return _Prefix; } }
        public object Value { get { return _Value; } }
        public object Suffix { get { return _Suffix; } }

        public SurroundedExpression(object prefix, object value, object suffix) {
            _Prefix = prefix;
            _Value = value;
            _Suffix = suffix;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder output) {
            output.Append(Prefix);
            output.Append(Value);
            output.Append(Suffix);
        }
    }
}
