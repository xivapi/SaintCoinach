using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class Parameter : IStringNode {
        private readonly ExpressionType _ParameterType;
        private readonly IStringNode _ParameterIndex;

        TagType IStringNode.Tag { get { return TagType.None; } }
        NodeType IStringNode.Type { get { return NodeType.Parameter; } }
        NodeFlags IStringNode.Flags { get { return NodeFlags.IsExpression; } }
        public ExpressionType ParameterType { get { return _ParameterType; } }
        public IStringNode ParameterIndex { get { return _ParameterIndex; } }

        public Parameter(ExpressionType parameterType, IStringNode parameterIndex) {
            if (parameterIndex == null)
                throw new ArgumentNullException("parameterIndex");
            _ParameterType = parameterType;
            _ParameterIndex = parameterIndex;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder builder) {
            builder.Append(ParameterType);
            builder.Append(StringTokens.ArgumentsOpen);
            ParameterIndex.ToString(builder);
            builder.Append(StringTokens.ArgumentsClose);
        }
    }
}
