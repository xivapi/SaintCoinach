using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak {
    public interface IContentPresenter : Behaviors.IRegionManagerAware {
        string TargetRegion { get; }
        Guid Id { get; }
    }
}
