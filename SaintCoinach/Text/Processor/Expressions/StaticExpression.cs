using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Processor.Expressions {
    public class StaticExpression : IExpression {
        private readonly object _Value;

        public object Value { get { return _Value; } }

        public StaticExpression(object value) {
            _Value = value;
        }
    }
}
