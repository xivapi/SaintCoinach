using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public interface IExpressionNode : INode {
        IExpression Evaluate(EvaluationParameters parameters);
    }
}
