using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Expressions {
    public class GenericExpression : IValueExpression {
        public static readonly GenericExpression Empty = new GenericExpression(null);

        private readonly object _Value;

        public object Value { get { return _Value; } }

        public GenericExpression(object value) {
            _Value = value;
        }

        public override string ToString() {
            if (Value == null)
                return string.Empty;
            return Value.ToString();
        }
        public void ToString(StringBuilder output) {
            output.Append(Value);
        }
    }
}
