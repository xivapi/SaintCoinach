using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class IfEqualsElement : IfElement {
        public IfEqualsElement(TagType tag, INode leftValue, INode rightValue, INode trueValue, INode falseValue)
            : base(tag, new Comparison(DecodeExpressionType.Equal, leftValue, rightValue), trueValue, rightValue) {
        }
    }
}
