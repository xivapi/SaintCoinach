using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class ArgumentCollection : IEnumerable<IStringNode> {
        private readonly IStringNode[] _Items;

        public bool HasItems { get { return _Items != null && _Items.Length > 0; } }
        public IEnumerable<IStringNode> Items { get { return _Items; } }

        public ArgumentCollection(IEnumerable<IStringNode> items) {
            if (items != null)
                _Items = items.ToArray();
        }

        public override string ToString() {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public void ToString(StringBuilder builder) {
            if (HasItems) {
                builder.Append(StringTokens.ArgumentsOpen);
                for (var i = 0; i < _Items.Length; ++i) {
                    if (i > 0)
                        builder.Append(StringTokens.ArgumentsSeperator);
                    _Items[i].ToString(builder);
                }
                builder.Append(StringTokens.ArgumentsClose);
            }
        }

        #region IEnumerable<IXivStringPart> Members

        public IEnumerator<IStringNode> GetEnumerator() {
            return Items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
