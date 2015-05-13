using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public class IfEqualsElement : IfElement {
        public IfEqualsElement(TagType tag, IStringNode leftValue, IStringNode rightValue, IStringNode trueValue, IStringNode falseValue)
            : base(tag, new Comparison(ExpressionType.Equal, leftValue, rightValue), trueValue, rightValue) {
        }
    }
}
