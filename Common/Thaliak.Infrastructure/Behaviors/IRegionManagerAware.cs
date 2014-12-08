using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Prism.Regions;

namespace Thaliak.Behaviors {
    public interface IRegionManagerAware {
        IRegionManager RegionManager { get; set; }
    }
}
