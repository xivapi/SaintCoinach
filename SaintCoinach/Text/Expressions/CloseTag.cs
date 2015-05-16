using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Expressions {
    public class CloseTag : IExpression {
        private readonly TagType _Tag;

        public TagType Tag { get { return _Tag; } }

        public CloseTag(TagType tag) {
            _Tag = tag;
        }

        #region IExpression Members

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder output) {
            output.Append(StringTokens.TagOpen);
            output.Append(StringTokens.ElementClose);
            output.Append(Tag);
            output.Append(StringTokens.TagClose);
        }

        #endregion
    }
}
