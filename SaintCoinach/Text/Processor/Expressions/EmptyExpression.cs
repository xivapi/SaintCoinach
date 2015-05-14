using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Processor.Expressions {
    public class EmptyExpression : IExpression {
        public static readonly EmptyExpression Instance = new EmptyExpression();
    }
}
