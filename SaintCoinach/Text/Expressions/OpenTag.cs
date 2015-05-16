using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Expressions {
    public class OpenTag : IExpression {
        private readonly TagType _Tag;
        private readonly IExpression[] _Arguments;

        public TagType Tag { get { return _Tag; } }
        public IEnumerable<IExpression> Arguments { get { return _Arguments; } }

        public OpenTag(TagType tag, IEnumerable<IExpression> arguments) {
            _Tag = tag;
            if (arguments == null)
                _Arguments = new IExpression[0];
            else
                _Arguments = arguments.ToArray();
        }

        #region IExpression Members

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder output) {
            output.Append(StringTokens.TagOpen);
            output.Append(Tag);

            if (_Arguments.Length > 0) {
                output.Append(StringTokens.ArgumentsOpen);
                for (var i = 0; i < _Arguments.Length; ++i) {
                    if (i > 0)
                        output.Append(StringTokens.ArgumentsSeperator);
                    _Arguments[i].ToString(output);
                }
                output.Append(StringTokens.ArgumentsClose);
            }

            output.Append(StringTokens.TagClose);
        }

        #endregion
    }
}
