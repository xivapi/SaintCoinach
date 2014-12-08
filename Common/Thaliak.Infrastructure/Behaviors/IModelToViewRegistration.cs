using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak.Behaviors {
    public interface IModelToViewRegistration {
        Type ModelType { get; }
        bool AllowDerivedTypes { get; }
    }
}
