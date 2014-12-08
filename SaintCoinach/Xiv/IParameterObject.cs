using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public interface IParameterObject {
        IEnumerable<Parameter> Parameters { get; }
    }
}
