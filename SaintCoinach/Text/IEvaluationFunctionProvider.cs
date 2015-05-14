using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    using Nodes;

    public interface IEvaluationFunctionProvider {
        bool Compare(EvaluationParameters parameters, DecodeExpressionType comparisonType, INode left, INode right);
        IExpression EvaluateGenericElement(EvaluationParameters parameters, Nodes.GenericElement element);
        bool ToBoolean(IExpression value);

        bool TryConvertToInteger(IExpression value, out int result);
        int ToInteger(IExpression value);
    }
}
