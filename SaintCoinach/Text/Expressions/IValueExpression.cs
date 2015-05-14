using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Expressions {
    public interface IValueExpression : IExpression {
        object Value { get; }
    }
}
