using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    [Flags]
    public enum NodeFlags {
        HasArguments,
        HasChildren,
        IsExpression,
        IsConditional,
        IsStatic,
    }
}
