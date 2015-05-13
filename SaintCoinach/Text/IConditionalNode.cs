using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    public interface IConditionalNode : IStringNode {
        IStringNode Condition { get; }
        IStringNode TrueValue { get; }
        IStringNode FalseValue { get; }
    }
}
